﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis.CodeStyle;

namespace Microsoft.CodeAnalysis.CSharp.CodeStyle
{
    internal static partial class CSharpCodeStyleOptions
    {
        public static CodeStyleOption<ExpressionBodyPreference> ParseExpressionBodyPreference(
            string optionString, CodeStyleOption<ExpressionBodyPreference> @default)
        {
            // optionString must be similar to true:error or when_on_single_line:suggestion.
            if (CodeStyleHelpers.TryGetCodeStyleValueAndOptionalNotification(optionString,
                    out var value, out var notificationOpt))
            {
                // A notification value must be provided.
                if (notificationOpt != null)
                {
                    if (bool.TryParse(value, out var boolValue))
                    {
                        return boolValue
                            ? new CodeStyleOption<ExpressionBodyPreference>(ExpressionBodyPreference.WhenPossible, notificationOpt)
                            : new CodeStyleOption<ExpressionBodyPreference>(ExpressionBodyPreference.Never, notificationOpt);
                    }

                    if (value == "when_on_single_line")
                    {
                        return new CodeStyleOption<ExpressionBodyPreference>(ExpressionBodyPreference.WhenOnSingleLine, notificationOpt);
                    }
                }
            }

            return @default;
        }

        private static string GetExpressionBodyPreferenceEditorConfigString(CodeStyleOption<ExpressionBodyPreference> value)
        {
            Debug.Assert(value.Notification != null);

            var notificationString = value.Notification.ToEditorConfigString();
            switch (value.Value)
            {
                case ExpressionBodyPreference.Never: return $"false:{notificationString}";
                case ExpressionBodyPreference.WhenPossible: return $"true:{notificationString}";
                case ExpressionBodyPreference.WhenOnSingleLine: return $"when_on_single_line:{notificationString}";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
