﻿namespace TreeFormatter
{
    using HtmlAgilityPack;
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Defines the <see cref="HtmlFormatter" />
    /// </summary>
    public static class HtmlFormatter
    {
        /// <summary>
        /// Minifies an html string
        /// </summary>
        /// <param name="source">source html <see cref="string"/></param>
        /// <returns>Minified html <see cref="string"/></returns>
        public static string MinifyHtml(this string source)
        {
            var redundantHtmlWhitespace = new Regex(@"(?<=>)\s+?(?=<)");
            return redundantHtmlWhitespace.Replace(source, String.Empty).Trim();
        }

        /// <summary>
        /// Prettifies an html string
        /// </summary>
        /// <param name="source">source html <see cref="string"/></param>
        /// <returns>Prettified html <see cref="string"/></returns>
        public static string PrettifyHtml(this string source)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(source);

            var stringBuilder = new StringBuilder();

            if (htmlDocument.DocumentNode != null)
                foreach (var node in htmlDocument.DocumentNode.ChildNodes)
                    stringBuilder = AppendNode(stringBuilder, node, 0);


            return stringBuilder.ToString();
        }

        /// <summary>
        /// Recursively appends html nodes
        /// </summary>
        /// <param name="stringBuilder">The stringBuilder<see cref="StringBuilder"/></param>
        /// <param name="node">The node<see cref="HtmlNode"/></param>
        /// <param name="indentLevel">The indentLevel<see cref="int"/></param>
        /// <returns>The <see cref="StringBuilder"/></returns>
        private static StringBuilder AppendNode(StringBuilder stringBuilder, HtmlNode node, int indentLevel)
        {
            // check parameter
            if (stringBuilder == null) return stringBuilder;
            if (node == null) return stringBuilder;

            // init 
            string INDENT = "  ";
            string NEW_LINE = System.Environment.NewLine;

            // case: no children
            if (node.HasChildNodes == false)
            {
                for (int i = 0; i < indentLevel; i++)
                    stringBuilder.Append(INDENT);
                stringBuilder.Append(node.OuterHtml);
                stringBuilder.Append(NEW_LINE);
            }

            // case: node has childs
            else
            {
                // indent
                for (int i = 0; i < indentLevel; i++)
                    stringBuilder.Append(INDENT);

                // open tag
                stringBuilder.Append(string.Format("<{0}", node.Name));
                if (node.HasAttributes)
                    foreach (var attr in node.Attributes)
                        stringBuilder.Append(string.Format("{0}=\"{1}\" ", attr.Name, attr.Value));
                stringBuilder.Append(string.Format(">{0}", NEW_LINE));

                // childs
                foreach (var chldNode in node.ChildNodes)
                    AppendNode(stringBuilder, chldNode, indentLevel + 1);

                // close tag
                for (int i = 0; i < indentLevel; i++)
                    stringBuilder.Append(INDENT);
                stringBuilder.Append(string.Format("</{0}>{1}", node.Name, NEW_LINE));
            }

            return stringBuilder;
        }
    }
}
