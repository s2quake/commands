Commands changes
===================

7.1.0
-------------

To be Released.

* Renamed method `GetCompletion` to `GetCompletions`.  [[#47]]
* Added a `ServiceProvider` property to the `CommandSettings`  [[#49]]
* Fixed an exception caused by empty data in the `TableDataBuilder`.  [[#52]]

[#47]: https://github.com/s2quake/commands/pull/47
[#49]: https://github.com/s2quake/commands/pull/49
[#52]: https://github.com/s2quake/commands/pull/52



7.0.0
-------------

Released on November 19, 2024.

* Added github action for CI.  [[#13]]
* Fixed cursor not updating when buffer height changed.  [[#17]]
* Fix an error where the system terminal fails to catch a TaskCanceledException.
  [[#19]]
* Added the feature to parse custom types.  [[#20]]
* Added Allows custom types to be used for method parameters  [[#22]]
* Added sub command of command  [[#23]]
* Added Added a variety of attributes that can be used for parameters.  [[#25]]
* Improved the structure to allow ICustomCommandDescriptor to be applied.
  [[#25]]
* Added validation to properties and parameter using DataAnnotation.  [[#26]]
* Fixed an issue with usage not printing correctly.  [[#28]]
* Removed the `CommandPropertyConditionAttribute`.  [[#31]]
* Added the `CommandPropertyDependencyAttribute`.  [[#31]]
* Added the `CommandPropertyExclusionAttribute`.  [[#31]]
* Added a `Category` property to the `CommandMemberDescriptor`.  [[#33]]
* Simplified the process of getting strings from `ResourceManager`.  [[#34]]
* Removed `ICommandUsage`, `CommandUsageDescriptorBase`,
  `ResourceUsageDescriptor`.  [[#34]]
* Fixed an issue with the display order of options and commands in Help.
  [[#37]]
* Removed commands sorting code in `CommandContextBase`.  [[#38]]
* Integrated usage-related properties(`Summary`, `Description`, `Example`) into
  the `CommandUsage` property.  [[#40]]
* Added `StepProgress` to make it easier to work with progress.  [[#41]]
* Added `RunAsync` and `StopAsync` methods to `SystemTerminalBase`.  [[#42]]
* Added `HelpCommand` and `VersionCommand` properties to `ICommandContext`.
  [[#43]]
* Added `PrintHelp` method to `HelpCommandBase`.  [[#43]]

[#13]: https://github.com/s2quake/commands/pull/13
[#17]: https://github.com/s2quake/commands/pull/17
[#19]: https://github.com/s2quake/commands/pull/19
[#20]: https://github.com/s2quake/commands/pull/20
[#22]: https://github.com/s2quake/commands/pull/22
[#23]: https://github.com/s2quake/commands/pull/23
[#25]: https://github.com/s2quake/commands/pull/25
[#26]: https://github.com/s2quake/commands/pull/26
[#28]: https://github.com/s2quake/commands/pull/28
[#31]: https://github.com/s2quake/commands/pull/31
[#33]: https://github.com/s2quake/commands/pull/33
[#34]: https://github.com/s2quake/commands/pull/34
[#37]: https://github.com/s2quake/commands/pull/37
[#38]: https://github.com/s2quake/commands/pull/38
[#40]: https://github.com/s2quake/commands/pull/40
[#41]: https://github.com/s2quake/commands/pull/41
[#42]: https://github.com/s2quake/commands/pull/42
[#43]: https://github.com/s2quake/commands/pull/43


6.0.1
-------------

Released on May 31, 2024.

* Fixed an issue where an exception was thrown when multiple
  commands using `PartialCommandAttribute` were inside an assembly.  [[#8]]

[#8]: https://github.com/s2quake/commands/pull/8

