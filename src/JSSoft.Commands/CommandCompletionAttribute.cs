// // <copyright file="CommandCompletionAttribute.cs" company="JSSoft">
// //   Copyright (c) 2024 Jeesu Choi. All Rights Reserved.
// //   Licensed under the MIT License. See LICENSE.md in the project root for license information.
// // </copyright>

// namespace JSSoft.Commands;

// [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
// public class CommandCompletionAttribute : CommandStaticTypeAttribute
// {
//     public CommandCompletionAttribute(string methodName)
//     {
//         MethodName = methodName;
//     }

//     public CommandCompletionAttribute(string staticTypeName, string methodName)
//         : base(staticTypeName)
//     {
//         MethodName = methodName;
//     }

//     public CommandCompletionAttribute(Type staticType, string methodName)
//         : base(staticType)
//     {
//         MethodName = methodName;
//     }

//     public string MethodName { get; }
// }
