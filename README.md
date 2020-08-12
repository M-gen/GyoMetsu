# GyoMetsu 魚滅の槍

## 概要

会話シーンと戦闘シーンで構成された、ショートRPG。
戦闘システムは、リアルタイムで、行動シンボルをためて行動する。

## 実行手順

実行には、素材ファイルの配置が必要です。仮素材を同梱しているので、下記の手順でコピーして下さい。

+ GyoMetsuフォルダ直下にbinフォルダを作ります
+ そのbinフォルダに、Debugフォルダを作ります(ReleseでビルドするときはRelese)
+ そのDebugフォルダに、dataフォルダを作ります()
+ そのdataフォルダに、Data/GithubPublicDatasのすべてのファイルをコピーします

## 目標

+ 3Dダンジョン部分を作って、探索できるようにする
+ プレーヤーキャラクターをウィズライクに作成できるようにする
+ プレーヤーキャラクターに応じて、会話の語尾や言い回し、主語を、変化させたい。
+ アイテムや武器、スキルなどのシステムを追加したい
+ 会話シーンで、音声合成による、テキストの読み上げ機能を追加
+ ゆくゆくは、ダンジョンRPGのゲーム制作エンジンにしたい

## 利用ツール

+ Visual Studio 2019 C#
+ Python 3.6.7
+ markdownlint [https://marketplace.visualstudio.com/items?itemName=DavidAnson.vscode-markdownlint]
