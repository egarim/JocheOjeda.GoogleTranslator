using DevExpress.ExpressApp.Utils;
using JocheOjeda.GoogleTranslator;
using System;
using System.Collections.Generic;

namespace JocheOjeda.XafTranslatorProvider
{
    public class JocheOjedaGoogleTranslator : TranslatorProviderBase
    {


        string GoogleApiCredentialsPath = string.Empty;
        public JocheOjedaGoogleTranslator(string GoogleApiCredentialsPath)
            : base("<br>", 2000)
        {

            this.GoogleApiCredentialsPath = GoogleApiCredentialsPath;
        }

        #region ITranslatorProvider Members
        public override string Caption
        {
            get { return "Joche Ojeda Google Translator"; }
        }
        public override string Description
        {
            get { return "Powered by <b>Joche Ojeda Google Translator</b>"; }
        }
        public override string[] GetLanguages()
        {
            string[] supportedLanguages = { "en", "fr", "de", "ru", "it", "es" };
            return supportedLanguages;
        }

        public override string Translate(string text, string sourceLanguageCode, string destinationLanguageCode)
        {


            return Translator.Translate(text, destinationLanguageCode, sourceLanguageCode, this.GoogleApiCredentialsPath);



        }
        #endregion
        public override IEnumerable<string> CalculateSentences(string text)
        {
            string[] leftSeparators = new string[] { "\"", "^'", " '", "('", "{" };
            string[] rightSeparators = new string[] { "\"", "'", "'", "'", "}" };
            int index = 0;
            int leftSeparatorIndex = 0;
            int rightSeparatorIndex = 0;
            int iSeparator = 0;
            int leftSeparatorSize = 0;
            while (index < text.Length)
            {
                leftSeparatorIndex = -1;
                for (int i = 0; i < leftSeparators.Length; i++)
                {
                    int separatorIndex = -1;
                    if (leftSeparators[i][0] == '^')
                    {
                        separatorIndex = text.IndexOf(leftSeparators[i].Substring(1), index);
                        if (separatorIndex == 0)
                        {
                            iSeparator = i;
                            leftSeparatorIndex = separatorIndex;
                            leftSeparatorSize = leftSeparators[i].Length - 1;
                        }
                    }
                    else
                    {
                        separatorIndex = text.IndexOf(leftSeparators[i], index);
                        if (separatorIndex >= 0 && (leftSeparatorIndex < 0 || separatorIndex < leftSeparatorIndex))
                        {
                            iSeparator = i;
                            leftSeparatorIndex = separatorIndex;
                            leftSeparatorSize = leftSeparators[i].Length;
                        }
                    }
                }
                if (leftSeparatorIndex >= 0)
                {
                    rightSeparatorIndex = text.IndexOf(rightSeparators[iSeparator], leftSeparatorIndex + leftSeparatorSize);
                    if (rightSeparatorIndex >= 0)
                    {
                        string result = text.Substring(index, leftSeparatorIndex - index).Trim();
                        if (result.Length > 0)
                        {
                            yield return result;
                        }
                        index = rightSeparatorIndex + rightSeparators[iSeparator].Length;
                        continue;
                    }
                }
                yield return text.Substring(index, text.Length - index).Trim();
                index = text.Length;
            }
        }
    }
}
