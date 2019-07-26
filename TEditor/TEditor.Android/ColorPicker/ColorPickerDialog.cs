using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using TEditor.Droid;

namespace MonoDroid.ColorPickers
{
    public class ColorPickerDialog : Dialog, View.IOnClickListener
    {
        public event ColorChangedEventHandler ColorChanged;

        private ColorPickerView _colorPicker;

        private ColorPickerPanelView _oldColor;
        private ColorPickerPanelView _newColor;

        public Color Color => _colorPicker.Color;

        public bool AlphaSliderVisible
        {
            get => _colorPicker.AlphaSliderVisible;
            set => _colorPicker.AlphaSliderVisible = value;
        }

        public ColorPickerDialog(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init(Color.BlanchedAlmond);
        }

        public ColorPickerDialog(Context context, Color initialColor) : base(context)
        {
            Init(initialColor);
        }

        private void Init(Color color)
        {
            // To fight color banding.
            Window.SetFormat(Format.Rgba8888);

            SetUp(color);
        }

        private void SetUp(Color color)
        {
            var inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);

            var layout = inflater.Inflate(Resource.Layout.dialog_color_picker, null);

            SetContentView(layout);

            SetTitle(Resource.String.dialog_color_picker);

            _colorPicker = layout.FindViewById<ColorPickerView>(Resource.Id.color_picker_view);
            _oldColor = layout.FindViewById<ColorPickerPanelView>(Resource.Id.old_color_panel);
            _newColor = layout.FindViewById<ColorPickerPanelView>(Resource.Id.new_color_panel);

            ((LinearLayout)_oldColor.Parent).SetPadding(
                (int)Math.Round(_colorPicker.DrawingOffset),
                0,
                (int)Math.Round(_colorPicker.DrawingOffset),
                0
            );

            _oldColor.SetOnClickListener(this);
            _newColor.SetOnClickListener(this);
            _colorPicker.ColorChanged += (sender, args) =>
                                             {
                                                 _newColor.Color = args.Color;
                                                 ColorChanged?.Invoke(this, new ColorChangedEventArgs { Color = _newColor.Color });
                                             };
            _oldColor.Color = color;
            _colorPicker.Color = color;
        }

        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.new_color_panel)
            {
                ColorChanged?.Invoke(this, new ColorChangedEventArgs { Color = _newColor.Color });
            }

            if (v.Id == Resource.Id.old_color_panel)
            {
                ColorChanged?.Invoke(this, new ColorChangedEventArgs { Color = _oldColor.Color });
            }
            GC.Collect();
            Dismiss();
        }

        public override Bundle OnSaveInstanceState()
        {
            var state = base.OnSaveInstanceState();
            state.PutInt("old_color", _oldColor.Color);
            state.PutInt("new_color", _newColor.Color);
            return state;
        }

        public override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            _oldColor.Color = new Color(savedInstanceState.GetInt("old_color"));
            _colorPicker.Color = new Color(savedInstanceState.GetInt("new_color"));
            base.OnRestoreInstanceState(savedInstanceState);
        }
    }
}