﻿using Orchard.Environment.Extensions;
using Orchard.Layouts.Framework.Display;
using Orchard.Layouts.Framework.Drivers;
using Orchard.Layouts.Framework.Elements;
using Orchard.Layouts.ViewModels;
using MarkdownElement = Orchard.Layouts.Elements.Markdown;

namespace Orchard.Layouts.Drivers {
    [OrchardFeature("Orchard.Layouts.Markdown")]
    public class MarkdownDriver : ElementDriver<MarkdownElement> {

        protected override EditorResult OnBuildEditor(MarkdownElement element, ElementEditorContext context) {
            var viewModel = new MarkdownEditorViewModel {
                Text = element.Content
            };
            var editor = context.ShapeFactory.EditorTemplate(TemplateName: "Elements.Markdown", Model: viewModel);

            if (context.Updater != null) {
                context.Updater.TryUpdateModel(viewModel, context.Prefix, null, null);
                element.Content = viewModel.Text;
            }
            
            return Editor(context, editor);
        }

        protected override void OnDisplaying(MarkdownElement element, ElementDisplayContext context) {
            context.ElementShape.ProcessedContent = ToHtml(element.Content);
        }

        protected override void OnIndexing(MarkdownElement element, ElementIndexingContext context) {
            context.DocumentIndex
                .Add("body", element.Content).RemoveTags().Analyze()
                .Add("format", "markdown").Store();
        }

        private string ToHtml(string markdown) {
            return new MarkdownSharp.Markdown().Transform(markdown);
        }
    }
}