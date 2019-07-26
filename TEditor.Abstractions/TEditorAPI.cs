using System;
using System.Threading.Tasks;

namespace TEditor.Abstractions
{
    partial class TEditor : ITEditorAPI
    {
        Func<string, Task<string>> _javaScriptEvaluatFuncWithResult;
        Action<string> _javaScriptEvaluatFunc;

        bool _platformIsDroid = false;

        public void SetJavaScriptEvaluatingFunction(Action<string> function)
        {
            if (function == null)
                throw new ArgumentNullException("Function cannot be null");
            _javaScriptEvaluatFunc = function;
        }

        public void SetJavaScriptEvaluatingWithResultFunction(Func<string, Task<string>> function)
        {
            if (function == null)
                throw new ArgumentNullException("Function cannot be null");
            _javaScriptEvaluatFuncWithResult = function;
        }
        public void UpdateHTML()
        {
            var html = InternalHTML;
            var cleanedHTML = RemoveQuotesFromHTML(html);
            var trigger = string.Format("zss_editor.setHTML(\"{0}\");", cleanedHTML);
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public async Task<string> GetHTML()
        {
            var html = await _javaScriptEvaluatFuncWithResult("zss_editor.getHTML();");
            return html;
        }

        string RemoveQuotesFromHTML(string html)
        {
            html = html.Replace("\"", "\\\"");
            html = html.Replace("“", "&quot;");
            html = html.Replace("”", "&quot;");
            html = html.Replace("\r", "\\r");
            html = html.Replace("\n", "\\n");
            return html;
        }

        async Task<string> TidyHTML(string html)
        {
            html = html.Replace("<br>", "<br />");
            html = html.Replace("<hr>", "<hr />");
            if (FormatHTML)
                html = await _javaScriptEvaluatFuncWithResult.Invoke(string.Format("style_html(\"{0}\");", html));
            return html;
        }

        public void InsertHTML(string html)
        {
            var cleanedHTML = RemoveQuotesFromHTML(html);
            var trigger = string.Format("zss_editor.insertHTML(\"{0}\");", cleanedHTML);
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void Focus()
        {
            var trigger = @"zss_editor.focusEditor();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void RemoveFormat()
        {
            var trigger = @"zss_editor.removeFormating();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void AlignLeft()
        {
            var trigger = @"zss_editor.setJustifyLeft();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void AlignCenter()
        {
            var trigger = @"zss_editor.setJustifyCenter();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void AlignRight()
        {
            var trigger = @"zss_editor.setJustifyRight();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void AlignFull()
        {
            var trigger = @"zss_editor.setJustifyFull();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetBold()
        {
            var trigger = @"zss_editor.setBold();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetItalic()
        {
            var trigger = @"zss_editor.setItalic();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetSubscript()
        {
            var trigger = @"zss_editor.setSubscript();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetUnderline()
        {
            var trigger = @"zss_editor.setUnderline();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetSuperscript()
        {
            var trigger = @"zss_editor.setSuperscript();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetStrikethrough()
        {
            var trigger = @"zss_editor.setStrikeThrough();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetUnorderedList()
        {
            var trigger = @"zss_editor.setUnorderedList();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetOrderedList()
        {
            var trigger = @"zss_editor.setOrderedList();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetHR()
        {
            var trigger = @"zss_editor.setHorizontalRule();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetIndent()
        {
            var trigger = @"zss_editor.setIndent();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetOutdent()
        {
            var trigger = @"zss_editor.setOutdent();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void Heading1()
        {
            var trigger = @"zss_editor.setHeading('h1');";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void Heading2()
        {
            var trigger = @"zss_editor.setHeading('h2');";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void Heading3()
        {
            var trigger = @"zss_editor.setHeading('h3');";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void Heading4()
        {
            var trigger = @"zss_editor.setHeading('h4');";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void Heading5()
        {
            var trigger = @"zss_editor.setHeading('h5');";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void Heading6()
        {
            var trigger = @"zss_editor.setHeading('h6');";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void Paragraph()
        {
            var trigger = @"zss_editor.setParagraph();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetPlatformAsIOS()
        {
            var trigger = @"zss_editor.setPlatformAsIOS();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetPlatformAsDroid()
        {
            var trigger = @"zss_editor.setPlatformAsDroid();";
            _javaScriptEvaluatFunc.Invoke(trigger);
            _platformIsDroid = true;
        }

        public void QuickLink()
        {
            var trigger = @"zss_editor.quickLink();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void Redo()
        {
            var trigger = @"zss_editor.redo();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetStrikeThrough()
        {
            var trigger = @"zss_editor.setStrikeThrough();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void Undo()
        {
            var trigger = @"zss_editor.undo();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetFooterHeight(double height)
        {
            var trigger = string.Format("zss_editor.setFooterHeight(\"{0:F}\");", height);
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetContentHeight(double height)
        {
            var trigger = string.Format("zss_editor.contentHeight = {0:F};", height);
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public Action LaunchColorPicker { get; set; }

        public void PrepareInsert()
        {
            var trigger = "zss_editor.prepareInsert();";
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

        public void SetTextColor(int R, int G, int B)
        {
            var trigger = string.Format("zss_editor.setTextColor(\"#{0:x2}{1:x2}{2:x2}\");", R, G, B);
            _javaScriptEvaluatFunc.Invoke(trigger);
        }

    }
}

