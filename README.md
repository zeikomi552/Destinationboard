# 行先ボードアプリ

ホワイトボードで運用していると思われる
行先ボード（行動予定表）をデジタル化してみようという試みです。

## 開発情報
### 開発環境
- Microsoft Visual Studio Community 2019
- .NET 5.0

### ビルド方法

以下の3つのプロジェクトを取得します。
- [Destinationboard](https://github.com/zeikomi552/Destinationboard)
- [DestinationboardServer](https://github.com/zeikomi552/DestinationboardServer)
- [DestinationboardCommunicationLibrary](https://github.com/zeikomi552/DestinationboardCommunicationLibrary)

すべて同階層に配置してください。

Microsoft Visual Studio Community 2019
を使用すればビルドが通ると思います

## 実行方法

### 起動方法
1. 実行ファイルの取得
以下の場所から実行ファイルを取得します
https://github.com/zeikomi552/Destinationboard/releases

2. 実行ファイルの配置
実行ファイルは以下の2種類に分かれています。

サーバー用アプリ
- DestinationboardServer

クライアント用アプリ
- Destinationboard

それぞれ任意のPCに配置してください。
とりあえず試したい場合は同じPCに配置するとすぐにお試しできます。

3. 設定ファイルの変更

Destinationboard\Config\Destination.confを変更します。

以下は設定ファイルの内容です。<p>
ServerNameはDestinationBoardServerを配置したPC名もしくはIPアドレスを設定します。<p>
Portは使用するPort番号を設定してください。デフォルトは552。<p>

```
<ConfigManager xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <ServerName>localhost</ServerName>
  <Port>552</Port>
</ConfigManager>
```

DestinationServer\Config\DestinationServer.confを変更します。

HostNameはlocalhost固定です。<p>
Portは使用するPort番号を設定してください。<p>
クライアントと同様のものである必要があります。<p>
SQLitePathはデータの保存先です。<p>
同梱のDestinationboardServer\db\DestinationBoard.dbを任意の場所においてそのパスを指定してください。<p>

```
<?xml version="1.0"?>
<ConfigManager xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <HostName>localhost</HostName>
  <Port>552</Port>
  <SQLitePath>.\db\DestinationBoard.db</SQLitePath>
</ConfigManager>
```

4. 起動順序

以下の順でアプリケーションを起動します。
- DestinationboardServer.exe
- Destinationboard.exe




