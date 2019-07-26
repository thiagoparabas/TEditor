﻿using CoreGraphics;
using System;
using UIKit;
using PointF = CoreGraphics.CGPoint;
using RectangleF = CoreGraphics.CGRect;

namespace PopColorPicker.iOS
{
    public class ColorPickerHSBView : UIView
    {
        private nfloat _hue;
        private nfloat _saturation;
        private nfloat _brightness;

        private byte _red;
        private byte _green;
        private byte _blue;

        private UIImageView _crosshairView;

        public ColorPickerHSBView(RectangleF frame)
            : base(frame)
        {
            _hue = 0f;
            _saturation = 1f;
            _brightness = 1f;

            SetCrosshairView();
            SetGestureRecognizer();
        }

        private void SetGestureRecognizer()
        {
            var gestureRecognizer = new UIPanGestureRecognizer(PanOrTapValue);
            AddGestureRecognizer(gestureRecognizer);
        }

        private void SetCrosshairView()
        {
            _crosshairView = new UIImageView(new UIImage("color-picker-inner-marker@2x.png"));
            var frame = _crosshairView.Frame;
            frame.X = Frame.Width - frame.Width / 2f;
            frame.Y = 0 - frame.Height / 2f;
            _crosshairView.Frame = frame;

            AddSubview(_crosshairView);
        }

        public byte Red
        {
            get => _red;
            set
            {
                _red = value;

                SetCrosshairPosition();
                CalculateHSB();
            }
        }

        public byte Green
        {
            get => _green;
            set
            {
                _green = value;

                SetCrosshairPosition();
                CalculateHSB();
            }
        }

        public byte Blue
        {
            get => _blue;
            set
            {
                _blue = value;

                SetCrosshairPosition();
                CalculateHSB();
            }
        }

        public delegate void ValueChanged(object sender, EventArgs e);

        public event ValueChanged Changed;

        protected virtual void OnChanged(EventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        public nfloat Hue
        {
            get => _hue;
            set
            {
                _hue = value;

                CalculateRGB();
                SetNeedsDisplay();

                OnChanged(EventArgs.Empty);
            }
        }

        public override void Draw(CGRect rect)
        {
            DrawColorSqure();
        }

        private void DrawColorSqure()
        {
            var startX = Bounds.GetMinX();
            var startY = Bounds.GetMinY();
            var endX = Bounds.GetMaxX();
            var endY = Bounds.GetMaxY();

            using (var context = UIGraphics.GetCurrentContext())
            {
                var path = new CGPath();
                path.AddLines(new PointF[]
                    {
                        new PointF(startX, startY),
                        new PointF(endX, startY),
                        new PointF(endX, endY),
                        new PointF(startX, endY)
                    });

                path.CloseSubpath();
                context.AddPath(path);
                context.Clip();

                using (var rgb = CGColorSpace.CreateDeviceRGB())
                {
                    UIColor.FromHSBA(_hue, 1f, 1f, 1f).GetRGBA(out var r, out var g, out var b, out _);

                    var gradient1 = new CGGradient(rgb, new CGColor[]
                        {
                            CGColorFromHex(0xff, 0xff, 0xff, 0xff),
                            CGColorFromHex((byte)(r * 0xff), (byte)(g * 0xff), (byte)(b * 0xff), 0xff)
                        });

                    var gradient2 = new CGGradient(rgb, new CGColor[]
                        {
                            CGColorFromHex(0x00, 0x00, 0x00, 0x0f),
                            CGColorFromHex(0x00, 0x00, 0x00, 0xff)
                        });

                    context.DrawLinearGradient(gradient1, new PointF(startX, startY), new PointF(endX, startX), 0);
                    context.DrawLinearGradient(gradient2, new PointF(startX, startY), new PointF(startY, endY), 0);
                }
            }
        }

        private CGColor CGColorFromHex(byte red, byte green, byte blue, byte alpha)
        {
            return UIColor.FromRGBA(red, green, blue, alpha).CGColor;
        }

        private void PanOrTapValue(UIGestureRecognizer recognizer)
        {
            var startX = Bounds.GetMinX();
            var startY = Bounds.GetMinY();
            var endX = Bounds.GetMaxX();
            var endY = Bounds.GetMaxY();

            switch (recognizer.State)
            {
                case UIGestureRecognizerState.Began:
                case UIGestureRecognizerState.Changed:
                case UIGestureRecognizerState.Ended:
                    var point = recognizer.LocationInView(this);
                    var pX = point.X;
                    var pY = point.Y;

                    if (point.X <= startX)
                        pX = startX;
                    else if (point.X >= endX)
                        pX = endX;

                    if (point.Y <= startY)
                        pY = startY;
                    else if (point.Y >= endY)
                        pY = endY;

                    _saturation = pX / endX;
                    _brightness = (nfloat)(Math.Abs(pY - endY) / endY);

                    SetCrosshairPosition();
                    CalculateRGB();
                    OnChanged(EventArgs.Empty);
                    break;

                case UIGestureRecognizerState.Failed:
                case UIGestureRecognizerState.Cancelled:
                    break;

                default:
                    break;
            }
        }

        private void SetCrosshairPosition()
        {
            var endX = Bounds.GetMaxX();
            var endY = Bounds.GetMaxY();
            var pX = _saturation * endX;
            var pY = (nfloat)Math.Abs((_brightness * endY) - endY);

            var frame = _crosshairView.Frame;
            frame.X = pX - frame.Width / 2f;
            frame.Y = pY - frame.Width / 2f;
            _crosshairView.Frame = frame;
        }

        private void CalculateRGB()
        {
            UIColor.FromHSBA(_hue, _saturation, _brightness, 1f).GetRGBA(out var r, out var g, out var b, out _);

            Red = (byte)(r * 0xff);
            Green = (byte)(g * 0xff);
            Blue = (byte)(b * 0xff);
        }

        private void CalculateHSB()
        {
            UIColor.FromRGBA(_red, _green, _blue, (byte)0xff).GetHSBA(out var h, out var s, out var b, out _);

            _hue = h;
            _saturation = s;
            _brightness = b;
        }
    }
}
