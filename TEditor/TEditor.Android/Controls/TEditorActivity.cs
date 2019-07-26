using Android.App;
using Android.Views;
using Android.Widget;
using System;
using TEditor.Abstractions;
using Xamarin.Forms.Platform.Android;
using Resource = TEditor.Droid.Resource;

namespace TEditor
{
    [Activity(Label = "TEditorActivity",
        WindowSoftInputMode = SoftInput.AdjustResize | SoftInput.StateHidden,
        Theme = "@style/Theme.AppCompat.Light.NoActionBar.FullScreen",
        ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class TEditorActivity : FormsAppCompatActivity
    {
        const int ToolbarFixHeight = 60;
        TEditorWebView _editorWebView;
        LinearLayoutDetectsSoftKeyboard _rootLayout;
        LinearLayout _toolbarLayout;
        Android.Support.V7.Widget.Toolbar _topToolBar;

        public static Action<bool, string> SetOutput { get; set; }

        protected override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.TEditorActivity);

            _topToolBar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.TopToolbar);
            _topToolBar.Title = CrossTEditor.PageTitle;
            _topToolBar.InflateMenu(Resource.Menu.TopToolbarMenu);
            _topToolBar.MenuItemClick += async (sender, e) =>
            {
                if (SetOutput != null)
                {
                    if (e.Item.TitleFormatted.ToString() == "Save")
                    {
                        var html = await _editorWebView.GetHTML();
                        SetOutput.Invoke(true, html);
                    }
                    else
                    {
                        SetOutput.Invoke(false, null);
                    }
                }

                Finish();
            };

            _rootLayout = FindViewById<LinearLayoutDetectsSoftKeyboard>(Resource.Id.RootRelativeLayout);
            _editorWebView = FindViewById<TEditorWebView>(Resource.Id.EditorWebView);
            _toolbarLayout = FindViewById<LinearLayout>(Resource.Id.ToolbarLayout);

            _rootLayout.onKeyboardShown += HandleSoftKeyboardShwon;
            _editorWebView.SetOnCreateContextMenuListener(this);

            BuildToolbar();

            var htmlString = Intent.GetStringExtra("HTMLString") ?? "<p></p>";
            _editorWebView.SetHTML(htmlString);

            var autoFocusInput = Intent.GetBooleanExtra("AutoFocusInput", false);
            _editorWebView.SetAutoFocusInput(autoFocusInput);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _rootLayout.onKeyboardShown -= HandleSoftKeyboardShwon;
        }

        public void BuildToolbar()
        {
            var builder = TEditorImplementation.ToolbarBuilder ?? new ToolbarBuilder().AddAll();

            foreach (var item in builder)
            {
                var imagebutton = new ImageButton(this);
                imagebutton.Click += (sender, e) => { item.ClickFunc?.Invoke(_editorWebView.RichTextEditor); };
                var imagename = item.ImagePath.Split('.')[0];
                var resourceId = (int)typeof(Resource.Drawable).GetField(imagename).GetValue(null);
                imagebutton.SetImageResource(resourceId);
                var toolbarItems = FindViewById<LinearLayout>(Resource.Id.ToolbarItemsLayout);
                toolbarItems.AddView(imagebutton);
            }
        }

        public void HandleSoftKeyboardShwon(bool shown, int newHeight)
        {
            if (shown)
            {
                _toolbarLayout.Visibility = ViewStates.Visible;
                var widthSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                var heightSpec = View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
                _toolbarLayout.Measure(widthSpec, heightSpec);
                var toolbarHeight = _toolbarLayout.MeasuredHeight == 0
                    ? (int)(ToolbarFixHeight * Resources.DisplayMetrics.Density)
                    : _toolbarLayout.MeasuredHeight;
                var topToolbarHeight = _topToolBar.MeasuredHeight == 0
                    ? (int)(ToolbarFixHeight * Resources.DisplayMetrics.Density)
                    : _topToolBar.MeasuredHeight;
                var editorHeight = newHeight - toolbarHeight - topToolbarHeight;
                _editorWebView.LayoutParameters.Height = editorHeight;
                _editorWebView.LayoutParameters.Width = ViewGroup.LayoutParams.MatchParent;
                _editorWebView.RequestLayout();
            }
            else
            {
                if (newHeight != 0)
                {
                    _toolbarLayout.Visibility = ViewStates.Invisible;
                    _editorWebView.LayoutParameters = new LinearLayout.LayoutParams(-1, -1);
                    ;
                }
            }
        }
    }
}