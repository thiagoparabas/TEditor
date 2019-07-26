using ObjCRuntime;
using System;
using System.Runtime.InteropServices;

namespace TEditor
{
    public class HideFormAccessoryBar
    {

        [DllImport("/usr/lib/libobjc.dylib")]
        static extern IntPtr class_getInstanceMethod(IntPtr classHandle, IntPtr Selector);

        [DllImport("/usr/lib/libobjc.dylib")]
        static extern IntPtr method_getImplementation(IntPtr method);

        [DllImport("/usr/lib/libobjc.dylib")]
        static extern IntPtr imp_implementationWithBlock(ref BlockLiteral block);

        [DllImport("/usr/lib/libobjc.dylib")]
        static extern void method_setImplementation(IntPtr method, IntPtr imp);

        private static IntPtr UIOriginalImp;
        private static IntPtr WKOriginalImp;
        private static bool _hideFormAccessoryBar;
        public static void SetHideFormAccessoryBar(bool hide)
        {
            if (hide == _hideFormAccessoryBar)
                return;
            var uiMethod = class_getInstanceMethod(Class.GetHandle("UIWebBrowserView"), new Selector("inputAccessoryView").Handle);
            var wkMethod = class_getInstanceMethod(Class.GetHandle("WKContentView"), new Selector("inputAccessoryView").Handle);

            if (hide)
            {
                UIOriginalImp = method_getImplementation(uiMethod);
                WKOriginalImp = method_getImplementation(wkMethod);

                var block_value = new BlockLiteral();
                CaptureDelegate d = MyCapture;
                block_value.SetupBlock(d, null);
                var nilimp = imp_implementationWithBlock(ref block_value);
                method_setImplementation(uiMethod, nilimp);
                method_setImplementation(wkMethod, nilimp);

            }
            else
            {
                method_setImplementation(uiMethod, UIOriginalImp);
                method_setImplementation(wkMethod, WKOriginalImp);
            }
            _hideFormAccessoryBar = hide;
        }

        private delegate IntPtr CaptureDelegate();

        [MonoPInvokeCallback(typeof(CaptureDelegate))]
        private static IntPtr MyCapture()
        {
            return IntPtr.Zero;
        }
    }
}
