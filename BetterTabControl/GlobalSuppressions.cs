
// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.

[module: System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Known bug https://github.com/dotnet/roslyn/issues/31744", Scope = "namespaceanddescendants", Target = "BetterTabs")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Parameter is an internal property name", Scope = "member", Target = "~M:BetterTabs.BetterTabControl.#ctor")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Is instantiated using WPF", Scope = "type", Target = "~M:BetterTabs.ScrollButtonsVisibilityConverter")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Is instantiated using WPF", Scope = "type", Target = "~M:BetterTabs.TabPresenterWidthCalculator")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812", Justification = "Is instantiated using WPF", Scope = "type", Target = "~M:BetterTabs.UniversalValueConverter")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801", Justification = "Reserved for future use", Scope = "member", Target = "~M:BetterTabs.Tab.OnIsDraggingChanged")]