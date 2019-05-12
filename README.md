# You have just found DM YGOPro2 in Unity 3D!

DM YGOPro2 is a card simulation game created in Unity Engine, with same protocol to ygopro.

This repository works with Hello23's ygopro-dm mod (https://github.com/Hello23-Ygopro/ygopro-dm).

# How to compile the game?

1. Download Unity 5.6.7 (https://unity3d.com/cn/get-unity/download/archive).

2. Clone the repository.

3. Double click Assets\main.unity to open the solution.

# How to compile the ocgcore wrapper?

*In most case you do not need to care about the ocgcore wrapper.*

1. Double click the **YGOProUnity_V2/AI_core_vs2017solution/core.sln**

2. build the c# solution in x64 and release mode and you get the **System.Servicemodel.Faltexception.dll**

3. copy it into **YGOProUnity_V2\Assets\Plugins**

*Yes, the name of the dll is System.Servicemodel.Faltexception.dll, though it does nothing with c# system :p*

# How to compile the ocgcore.dll?

*In most case you do not need to care about the ocgcore.dll.*

1. Double click the **YGOProUnity_V2/AI_core_vs2017solution/core.sln**

2. build the c++ solution in x64 and release mode and you get the **ocgcore.dll**

3. copy it into **YGOProUnity_V2\Assets\Plugins**

# Linux Or Mac install libgdiplus

Ubuntu
`
apt intsall libgdiplus
`

Mac OSX
`
brew install mono-libgdiplus
`

or

Download [Mono](https://download.mono-project.com/archive/5.16.0/macos-10-universal/MonoFramework-MDK-5.16.0.220.macos10.xamarin.universal.pkg)
