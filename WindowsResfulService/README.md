# CsRESTService
RESTful Web API Service with C# in Windows Service

2016-02-24 Mitsuru Horigome

# 1, Overview

RESTful Web APIをC#で実装し、Windowsサービスにするサンプルコードです。
WCFを使って実装しています。

# 2, Requirement

* VisualStudio 2013
* .NET Framework 4.5

# 3, How To Build

CsHttpTest.sln を開き、ソリューションのビルドを実行します。

# 4, Projects

## CsRESTConsole

http WebAPIをコマンドラインで実行するサンプルコードです。  
実行するには、管理者権限で実行します。  コンソールが開いたら、次のURIで実行します。

    [GET] http://localhost:8081/api/version
    [GET] http://localhost:8081/api/image
    
## CsRESRWinSvc

CsRESTConsoleと同じサービスをWindows Serviceとして実装したものです。  
Buildしたら、管理者権限のコマンドラインで次のようにインストールします。

    $ CsRESTWinSvc /i         ... install
    $ CsRESRWinSvc /u         ... uninstall
 
 サービスをインストールすると、"CsRESTWinSvs"というサービス名で登録され、起動状態になります。   
 URIは、同じです。
 
    [GET] http://localhost:8081/api/version
    [GET] http://localhost:8081/api/image

# License

MIT.
