## C#とDxLibで作る！ シンプル横スクロールアクションゲーム講座

**対象:** 高校1年生（プログラミング初学者）
**使用言語:** C#
**使用ライブラリ:** DxLib

---

### 目次

*   **第1章：はじめに - ゲーム作りの第一歩**
    *   ゲームってなんだろう？ 横スクロールアクションゲームとは？
    *   C#とDxLibについて
    *   プロジェクトの全体像（ファイル構成）
    *   プログラムの入口とゲームの心臓部 (`Program.cs`, `Game.cs`)
    *   第1章のまとめと質問

*   **第2章：ゲームの世界を決める - 定数とデータ**
    *   定数ってなに？ (`Constants.cs`)
    *   ゲームに必要な「決めごと」 (プレイヤーの速さ、画面サイズなど)
    *   ゲームの部品（画像）と設計図（マップ）を読み込む
    *   「静的（static）」ってどういうこと？
    *   第2章のまとめと質問

*   **第3章：地面を描こう - 背景スクロールの仕組み**
    *   地面を描く担当者 (`Ground.cs`)
    *   ゲーム世界の住所と画面上の住所 (座標系)
    *   プレイヤーを追いかけるカメラ（横スクロール）
    *   タイルマップ：小さな四角で世界を作る
    *   第3章のまとめと質問

*   **第4章：主人公登場！ - プレイヤーの表示**
    *   プレイヤーって何者？ (`Player.cs`)
    *   クラスとオブジェクト：設計図と実体
    *   プレイヤーの情報（位置、状態、見た目）
    *   画面への描き方 (`Player.Render`)
    *   第4章のまとめと質問

*   **第5章：プレイヤーを動かす - キーボード入力**
    *   プレイヤーを更新する (`Player.Update`)
    *   キーボードの入力を受け取る (`Game.GetCurrentInput`, `InputState`)
    *   入力に合わせて動きを決める (`Player.HandleInput`)
    *   `Vector2`：位置や動きをまとめて扱う便利な道具
    *   第5章のまとめと質問

*   **第6章：ジャンプと重力 - 物理法則の再現**
    *   重力を再現する (`Player.ApplyGravity`)
    *   ジャンプ！ (`Player.Jump`)
    *   速度と位置の関係
    *   第6章のまとめと質問

*   **第7章：壁と床 - 衝突判定（基本編）**
    *   なぜ衝突判定が必要？
    *   タイルとの当たり判定 (`Constants.IsSolidTile`)
    *   横方向の衝突 (`Player.ResolveCollisionHorizontal`)
    *   ぶつかったら止める（位置の調整）
    *   第7章のまとめと質問

*   **第8章：壁と床 - 衝突判定（応用編）**
    *   縦方向の衝突 (`Player.ResolveCollisionVertical`)
    *   地面に着地したときの処理
    *   天井に頭をぶつけたときの処理
    *   マップから落ちたら？
    *   第8章のまとめと質問

*   **第9章：プレイヤーを生き生きと - アニメーション**
    *   アニメーションの仕組み (`Player.UpdateAnimation`)
    *   状態に合わせた画像の切り替え（止まる、歩く、ジャンプ）
    *   パラパラ漫画の原理
    *   第9章のまとめと最終課題

---

### 第1章：はじめに - ゲーム作りの第一歩

**この章の目標:**
*   これから作るゲームのイメージをつかむ。
*   C#とDxLibがどんなものか知る。
*   プログラム全体の流れを理解する。

#### ゲームってなんだろう？ 横スクロールアクションゲームとは？

みんなが普段遊んでいるゲーム。スマートフォンやゲーム機、パソコンで色々な種類のゲームがあるよね。今回作るのは、その中でも「横スクロールアクションゲーム」と呼ばれるものだよ。

*   **横スクロール:** 画面がキャラクターの動きに合わせて横に流れていくタイプ。マリオブラザーズなどを想像してみてね。
*   **アクションゲーム:** キャラクターを操作して、ジャンプしたり、移動したり、敵を倒したり（今回は移動とジャンプだけだけど）する、動きのあるゲームのこと。

![](./placeholder_images/platformer_concept.png)
*図1-1: 横スクロールアクションゲームのイメージ図*

#### C#とDxLibについて

*   **C# (シーシャープ):**
    *   マイクロソフトが開発したプログラミング言語。
    *   Windowsアプリやゲーム、Webサービスなど、色々なものを作るのに使われているよ。
    *   「オブジェクト指向」という考え方に基づいていて、プログラムを部品（オブジェクト）の集まりとして作るのが得意なんだ。これは後で詳しく見ていこう。

*   **DxLib (ディーエックスライブラリ):**
    *   日本の開発者が作った、ゲーム作りに便利な機能がたくさん詰まった道具箱（ライブラリ）のようなもの。
    *   これを使うと、画面に絵を表示したり、キーボードからの入力を受け取ったり、音楽を鳴らしたりするのが簡単にできるんだ。
    *   今回はこのDxLibを使って、C#でゲームを作っていくよ。

#### プロジェクトの全体像（ファイル構成）

今回のゲームは、いくつかのC#ファイル（`.cs`という拡張子がついている）に役割分担させて作られているよ。

*   `Constants.cs`: ゲーム全体で使う「決めごと」（定数）をまとめておくファイル。
*   `Program.cs`: プログラムの実行が始まる場所。DxLibの準備もここで行う。
*   `Game.cs`: ゲーム全体の進行役。ゲームのメインループを管理する。
*   `Ground.cs`: 地面（マップ）を描画する担当。
*   `Player.cs`: 主人公（プレイヤーキャラクター）に関する全ての処理を担当。

このように、役割ごとにファイルを分けることで、プログラムが整理されて分かりやすくなるんだ。

```
プロジェクトフォルダ
│
├── Assets          <- 画像やマップデータ置き場
│   ├── tileset_ramina.png
│   ├── tileset_ground.png
│   └── tilemap.csv
│
├── Constants.cs    <- 定数
├── Program.cs      <- プログラムの開始点、DxLib初期化
├── Game.cs         <- ゲーム全体の流れ
├── Ground.cs       <- 地面の描画
└── Player.cs       <- プレイヤーの処理
```
*図1-2: プロジェクトのフォルダ構成*

#### プログラムの入口とゲームの心臓部 (`Program.cs`, `Game.cs`)

*   **`Program.cs`:**
    *   `Main`メソッド: 全てのC#プログラムは、ここから実行が始まる「入口」。
    *   DxLibの初期化: `DX.DxLib_Init()` という命令で、DxLibを使う準備をしている。ゲームを作る前に必ず必要なおまじないのようなものだと思ってね。画面の設定などもここで行う。
    *   `new Game().Run()`: `Game`クラスの設計図から実際のゲームオブジェクトを作り、その`Run`メソッド（ゲームを開始する命令）を呼び出している。

*   **`Game.cs`:**
    *   `Game`クラス: ゲーム全体の管理を担当する設計図。
    *   `Run`メソッド: ここがゲームの「心臓部」である**ゲームループ**。
        *   ゲームループとは、ゲームが終わるまで「入力受付 → 状態更新 → 画面描画」という処理を高速で繰り返すこと。
        *   `while (DX.ProcessMessage() == 0 && DX.CheckHitKey(DX.KEY_INPUT_ESCAPE) == 0)`: 「ウィンドウが閉じられていない」かつ「Escapeキーが押されていない」間、ずっと{}の中を繰り返すという意味。
        *   `Update()`: ゲームの状態を更新する（プレイヤーの位置を動かすなど）。
        *   `Render()`: 更新された状態を画面に描画する。

```csharp
// Game.cs より抜粋
public void Run()
{
    // ゲームループ
    while (DX.ProcessMessage() == 0 && DX.CheckHitKey(DX.KEY_INPUT_ESCAPE) == 0)
    {
        // 1. 状態を更新する (Update)
        Update();
        // 2. 画面を描画する (Render)
        Render();
    }
}
```

**ポイント:** ゲームは「更新」と「描画」をひたすら繰り返すことで動いている！

#### 第1章のまとめと質問

*   横スクロールアクションゲームの基本的なイメージを理解した。
*   C#はプログラミング言語、DxLibはゲーム作りを助けるライブラリであることを学んだ。
*   プログラムが複数のファイルに分かれて役割分担していることを知った。
*   ゲームは「ゲームループ」によって動いており、「更新(Update)」と「描画(Render)」を繰り返すことを学んだ。

**質問タイム！**

1.  今回作るゲームはどんな種類のゲームかな？
2.  DxLibを使うと、どんなことが簡単にできるようになる？
3.  ゲームループの中で繰り返される主な2つの処理は何かな？

---

### 第2章：ゲームの世界を決める - 定数とデータ

**この章の目標:**
*   定数を使うメリットを理解する。
*   ゲームに必要な様々な値を設定する。
*   画像やマップデータをプログラムに読み込む方法を学ぶ。
*   `static`キーワードの基本的な意味を知る。

#### 定数ってなに？ (`Constants.cs`)

プログラムを書いていると、同じ値を色々な場所で使うことがある。例えば、「プレイヤーの歩く速さ」や「画面の横幅」など。これらの値を直接数字で書く（例えば `3` や `640`）こともできるけど、それだと後で変更するのが大変だし、その数字が何を表しているのか分かりにくくなる。

そこで使うのが**定数 (Constant)** や **静的読み取り専用変数 (Static Readonly Variable)** だ。

*   **定数 (`const`)**:
    *   プログラムの実行前に値が決まっていて、**絶対に変わらない**値。
    *   例: `public const int PLAYER_SPEED = 3;` (プレイヤーの速度は3で、絶対変わらない)

*   **静的読み取り専用変数 (`static readonly`)**:
    *   プログラムが動き始めて**一度だけ値を設定でき、その後は変更できない**値。
    *   例: `public static readonly Vector2 SCREEN_SIZE;` (画面サイズ。後で計算して設定するけど、一度決めたら変えない)
    *   `static readonly` は、`const` では扱えない複雑な型の値や、実行時に計算が必要な値を設定したいときに使う。

`Constants.cs`ファイルには、このようにゲーム全体で使う「決めごと」となる値をまとめて定義しておくんだ。

```csharp
// Constants.cs より抜粋
internal static class Constants
{
    // --- Player Constants ---
    public const int PLAYER_SPEED = 3;     // プレイヤーの歩く速さ
    public const int JUMP_POWER = -20;   // ジャンプ力 (マイナスは上方向)
    public const int GRAVITY_INCREMENT = 1; // 重力の強さ

    // --- Screen Constants ---
    public static readonly Vector2 SCREEN_SIZE; // 画面サイズ (後で設定)

    // ... 他の定数 ...

    // この部分で SCREEN_SIZE などに実際の値を入れている
    static Constants()
    {
        SCREEN_SIZE = new Vector2(640f, 480f); // 画面サイズを横640, 縦480に設定
        // ... 他の初期化 ...
    }
}
```

**ポイント:** 定数を使うと、値の意味が分かりやすくなり、後からの変更も楽になる！

#### ゲームに必要な「決めごと」 (プレイヤーの速さ、画面サイズなど)

`Constants.cs`の中を見ると、色々な「決めごと」が定義されているね。

*   `PLAYER_SPEED`: プレイヤーが1フレーム（ゲームループの1周）あたりに左右に動くピクセル数。
*   `JUMP_POWER`: ジャンプした瞬間にプレイヤーにかかる上向きの力。値がマイナスなのは、ゲームの座標系では通常、画面の上方向がY軸のマイナス方向だからだよ（後で詳しく説明するね）。
*   `GRAVITY_INCREMENT`: 1フレームごとにプレイヤーの下向きの速度がどれだけ増えるか。これが重力を表現している。
*   `GROUND_SIZE`: 地面タイルの1辺の大きさ（ピクセル数）。
*   `SCREEN_SIZE`: ゲーム画面の大きさ（横幅と縦幅）。`Vector2`という型で、X（横）とY（縦）の値をペアで保持している。
*   `PLAYER_SIZE`: プレイヤー画像の大きさ。
*   `MAP_WIDTH`, `MAP_HEIGHT`: マップ全体のタイルの数（横と縦）。これはマップファイルを読み込んでから計算される。

これらの値を調整することで、ゲームの難易度や操作感を変えることができるんだ。

#### ゲームの部品（画像）と設計図（マップ）を読み込む

ゲームには、キャラクターや背景の**画像 (Sprite)** と、ステージの形を決める**マップデータ (Map Data)** が必要だね。これらは通常、別のファイルとして用意しておき、プログラム起動時に読み込む。

*   **画像ファイルの読み込み (`LoadSprites`)**:
    *   `Program.cs` に `LoadSprites` という、画像を読み込むための特別な命令（メソッド）が用意されている。
    *   これは、1枚の大きな画像（スプライトシート）に並んだ複数のキャラクターやタイルの絵を、プログラムで使いやすいようにバラバラに分割して読み込む機能。
    *   `Constants.cs` の中で、この `LoadSprites` を使ってプレイヤー画像 (`PLAYER_IMAGES`) と地面画像 (`GROUND_IMAGES`) を読み込んでいる。
    *   読み込まれた各画像には、DxLibが管理するための番号（ハンドル）が割り当てられる。この番号を使って、後で画面に画像を描画するんだ。

```csharp
// Constants.cs の static コンストラクタ内より抜粋
PLAYER_IMAGES = Program.LoadSprites(@"tileset_ramina.png", 3, 2, (int)PLAYER_SIZE.X, (int)PLAYER_SIZE.Y);
GROUND_IMAGES = Program.LoadSprites(@"tileset_ground.png", 5, 3, 16, 16);
```
*図2-1: `LoadSprites` で画像を分割・読み込みするイメージ*
![](./placeholder_images/loadsprites.png)

*   **マップデータの読み込み (`LoadMapData`)**:
    *   `Constants.cs` に `LoadMapData` というメソッドがある。
    *   これは、`tilemap.csv` というファイルを読み込むためのもの。
    *   CSVファイルは、カンマ(`,`)で区切られたテキストファイルで、今回はマップの設計図として使われている。各数字が、その場所にどの種類の地面タイルを置くか（または何も置かないか）を表しているんだ。
    *   `StreamReader` という道具を使ってファイルを開き、1行ずつ読み込み、カンマで分割 (`Split(',')`) し、数字に変換 (`int.TryParse`) して、`List<List<int>>` という二次元リスト（表のようなデータ構造）に格納している。これが `MAP_DATA` になる。

```csv
// tilemap.csv の例 (一部)
0,0,0,0,0,0,0,0,0,0,...
0,0,0,0,0,0,0,0,0,0,...
0,0,0,0,0,0,0,0,0,0,...
1,1,1,1,1,1,1,1,1,1,...  <- 地面タイル(例: 1番)
2,2,2,2,2,2,2,2,2,2,...  <- 別の地面タイル(例: 2番)
```
*図2-2: CSVマップデータのイメージ*

#### 「静的（static）」ってどういうこと？

`Constants.cs`, `Program.cs`, `Ground.cs` に `static` というキーワードが付いているね。これはどういう意味だろう？

通常、クラス（設計図）からオブジェクト（実体）を作って使う（例: `Game game = new Game();`）。でも、`static` が付いたクラスやメソッドは、**オブジェクトを作らなくても、クラス名から直接呼び出して使える**んだ。

*   **`static class`**: このクラス自体が、プログラム全体でただ一つの、共通の存在になる。`Constants` や `Ground` のように、ゲーム全体で共有する情報や機能を持たせたい場合に使う。
*   **`static` メソッド/変数**: クラス名を指定すれば、どこからでも `クラス名.メソッド名` や `クラス名.変数名` の形でアクセスできる。`Program.LoadSprites` や `Constants.PLAYER_SPEED` などがこれにあたる。

**イメージ:**
*   普通のクラス: たい焼きの型（クラス）。たくさんたい焼き（オブジェクト）を作れる。
*   `static`なクラス/メソッド: 学校の放送室（staticクラス）や、校内放送（staticメソッド）。学校に一つしかなく、どの教室からでも利用できる。

#### 第2章のまとめと質問

*   定数を使うと、コードが分かりやすく、変更しやすくなることを学んだ。
*   `const` は絶対不変、`static readonly` は初期化時のみ設定可能な値であることを知った。
*   ゲームに必要なパラメータ（速度、サイズなど）が `Constants.cs` にまとめられているのを見た。
*   `LoadSprites` で画像ファイルを、`LoadMapData` でCSVのマップデータを読み込む方法を学んだ。
*   `static` キーワードは、オブジェクトを作らずにクラス名から直接使える機能やデータを定義するものであることを理解した。

**質問タイム！**

1.  `PLAYER_SPEED` の値を `3` から `5` に変えたら、ゲームはどう変化するかな？
2.  画像ファイル(`tileset_ground.png`)やマップファイル(`tilemap.csv`)は、プロジェクトフォルダの中のどこに置かれているかな？ (`Constants.cs` の `ASSET_PATH` や `LoadMapData` のパスを見てみよう)
3.  `Constants.SCREEN_SIZE` にアクセスしたいとき、`Constants` クラスのオブジェクトを作る必要はあるかな？ なぜ？

---

### 第3章：地面を描こう - 背景スクロールの仕組み

**この章の目標:**
*   ゲームの座標系（ワールド座標とスクリーン座標）を理解する。
*   背景がプレイヤーに合わせてスクロールする仕組みを学ぶ。
*   タイルマップを描画する方法を理解する。

#### 地面を描く担当者 (`Ground.cs`)

`Ground.cs` ファイルの役割は、第2章で読み込んだマップデータ (`Constants.MAP_DATA`) と地面画像 (`Constants.GROUND_IMAGES`) を使って、ゲーム画面に背景となる地面（ステージ）を描画することだよ。

このクラスも `static` になっているので、`Game.cs` の `Render` メソッドから `Ground.Render(Player.Position);` のように直接呼び出して使うことができる。

#### ゲーム世界の住所と画面上の住所 (座標系)

ゲームを作る上で、**座標 (Coordinate)** の考え方はとても重要だ。今回は2つの座標系を意識する必要があるよ。

*   **ワールド座標 (World Coordinates):**
    *   ゲームの世界全体の広大なマップ上での位置を表す座標。プレイヤーや敵キャラクターなどは、このワールド座標で自分の位置を持っている。
    *   原点 (0, 0) はマップの左上隅になることが多い。X軸は右方向がプラス、Y軸は**下方向がプラス**になるのが一般的（数学のグラフとはY軸の向きが逆なのに注意！）。
    *   プレイヤーはワールド座標 (100, 100) の位置からスタートする、といった感じ。

*   **スクリーン座標 (Screen Coordinates):**
    *   実際にプレイヤーが見ているゲーム画面上での位置を表す座標。
    *   画面の左上隅が原点 (0, 0)。X軸は右方向がプラス、Y軸は下方向がプラス。
    *   画面のサイズは `Constants.SCREEN_SIZE` (例えば 640x480 ピクセル) で決まっている。
    *   最終的に画面に何かを描画するときは、このスクリーン座標を指定する必要がある。

![](./placeholder_images/coordinates.png)
*図3-1: ワールド座標とスクリーン座標のイメージ*

#### プレイヤーを追いかけるカメラ（横スクロール）

横スクロールゲームでは、プレイヤーがマップの端から端まで移動できるように、プレイヤーの動きに合わせて**カメラ**が移動し、画面に映る範囲が変わるように見えるよね。

`Ground.Render` メソッドの中では、このカメラの動きをシミュレートしているんだ。

```csharp
// Ground.cs の Render メソッドより抜粋
public static void Render(Vector2 playerPosition)
{
    // カメラの左上のワールド座標を計算
    // プレイヤーが画面の中央に来るようにカメラのX座標を決める
    float cameraX = playerPosition.X - SCREEN_SIZE.X / 2f;
    // Y座標は今回は固定（縦スクロールなし）
    float cameraY = 0f;

    // ... (この後、cameraX, cameraY を使って描画範囲を決める) ...
}
```

ここで計算している `cameraX`, `cameraY` は、「**ワールド座標のうち、画面の左上に表示されるべき部分の座標**」を表している。プレイヤーが右に移動すれば `playerPosition.X` が増えるので `cameraX` も増え、カメラが右に移動する（＝背景が左にスクロールして見える）という仕組みだ。

#### タイルマップ：小さな四角で世界を作る

マップデータ (`MAP_DATA`) は、数字の二次元リストだったね。この数字は、マップ上の各マス（タイル）にどの画像を表示するかを示している。`Ground.Render` では、このマップデータを使ってタイルを一つずつ描画していくよ。

でも、マップ全体を毎回全部描画するのは無駄が多い。画面に映っている部分だけ描画すれば十分だよね。

1.  **描画範囲の計算:**
    *   `cameraX`, `cameraY` と画面サイズ (`SCREEN_SIZE`) を使って、画面に映る可能性のあるタイルの範囲（開始タイルX座標 `startTileX` から終了タイルX座標 `endTileX` まで、Y座標も同様）を計算する。
    *   `Math.Floor` は小数点以下を切り捨てる計算。タイルの座標は整数だからね。
    *   `GROUND_SIZE` はタイル1枚の大きさ。`cameraX / GROUND_SIZE` で、カメラの左端が何番目のタイルに位置するかを計算している。

```csharp
// Ground.cs の Render メソッドより抜粋

// 画面に映るタイルのX座標の範囲を計算
int startTileX = (int)Math.Floor(cameraX / GROUND_SIZE);
int endTileX = (int)Math.Floor((cameraX + SCREEN_SIZE.X) / GROUND_SIZE) + 1; // +1で画面右端に少し見えるタイルも描画

// Y座標の範囲（今回は縦スクロールしないので全範囲）
int startTileY = 0;
int endTileY = MAP_HEIGHT;

// 範囲がマップの外にはみ出さないように調整 (Clamp)
startTileX = Math.Max(0, startTileX); // 0より小さくならないように
endTileX = Math.Min(MAP_WIDTH, endTileX); // マップ幅を超えないように
startTileY = Math.Max(0, startTileY);
endTileY = Math.Min(MAP_HEIGHT, endTileY);
```

2.  **タイルの描画:**
    *   計算した範囲 (`startTileX` から `endTileX` まで、`startTileY` から `endTileY` まで) のタイルを、`for`ループを使って一つずつ見ていく。
    *   `MAP_DATA[y][x]` で、その場所(x, y)に置くべきタイルの種類を示す番号 (`tileIndex`) を取得する。
    *   `tileIndex` が 0 (何もない場所) や無効な番号でなければ、対応する地面画像 (`GROUND_IMAGES[tileIndex]`) を取得する。
    *   タイルの**ワールド座標** (`x * GROUND_SIZE`, `y * GROUND_SIZE`) から**カメラの座標** (`cameraX`, `cameraY`) を引くことで、タイルの**スクリーン座標** (`screenX`, `screenY`) を計算する。
    *   `DX.DrawExtendGraph` 命令を使って、計算したスクリーン座標に、指定した大きさ (`GROUND_SIZE`) でタイル画像を描画する。`DX.TRUE` は透明色を有効にする設定。

```csharp
// Ground.cs の Render メソッドより抜粋
for (int y = startTileY; y < endTileY; y++)
{
    for (int x = startTileX; x < endTileX; x++)
    {
        int tileIndex = MAP_DATA[y][x];
        if (tileIndex <= 0 || tileIndex >= GROUND_IMAGES.Length) continue; // 空白タイルや無効な番号はスキップ

        int imageHandle = GROUND_IMAGES[tileIndex];
        if (imageHandle < 0) continue; // 画像読み込み失敗もスキップ

        // タイルのスクリーン座標を計算
        float screenX = x * GROUND_SIZE - cameraX;
        float screenY = y * GROUND_SIZE - cameraY;

        // タイルを描画
        DX.DrawExtendGraph(
            (int)screenX, (int)screenY,                         // 描画開始座標 (左上)
            (int)(screenX + GROUND_SIZE), (int)(screenY + GROUND_SIZE), // 描画終了座標 (右下)
            imageHandle,                                        // 描画する画像の番号
            DX.TRUE                                             // 透明色を有効にするか
        );
    }
}
```

**ポイント:** ワールド座標とスクリーン座標を理解し、カメラの動きに合わせて描画範囲と描画位置を計算することが、スクロールとタイルマップ描画の鍵！

#### 第3章のまとめと質問

*   ゲーム内の位置を表す「ワールド座標」と、画面上の位置を表す「スクリーン座標」の違いを学んだ。
*   プレイヤーの位置に合わせてカメラの座標 (`cameraX`) を計算することで、背景がスクロールする仕組みを理解した。
*   マップデータ (`MAP_DATA`) とタイル画像 (`GROUND_IMAGES`) を使い、画面に映る範囲のタイルだけを選んで描画する方法を学んだ。
*   ワールド座標からスクリーン座標への変換方法（カメラ座標を引く）を知った。

**質問タイム！**

1.  プレイヤーがゲーム世界の右端に近づいていくと、`cameraX` の値はどうなるかな？ それによって `screenX` の値はどう変化するかな？
2.  もし縦方向にもスクロールさせたい場合、`cameraY` はどのように計算すればよいだろう？
3.  `MAP_DATA[y][x]` の値が `0` の場合、なぜ `continue;` で処理をスキップするのかな？

---

### 第4章：主人公登場！ - プレイヤーの表示

**この章の目標:**
*   クラスとオブジェクトの基本的な考え方を理解する。
*   プレイヤーが持つべき情報（プロパティ）を知る。
*   プレイヤーを画面の特定の位置に描画する方法を学ぶ。

#### プレイヤーって何者？ (`Player.cs`)

いよいよ主人公であるプレイヤーキャラクターの登場だ！ `Player.cs` ファイルには、プレイヤーに関するすべての情報と操作（動き、描画、当たり判定など）が詰め込まれている。これは**クラス (Class)** として定義されているよ。

#### クラスとオブジェクト：設計図と実体

ここで、プログラミングの重要な考え方「**オブジェクト指向**」の基本に触れてみよう。

*   **クラス (Class):**
    *   モノの**設計図**や**テンプレート**のようなもの。
    *   例えば、「プレイヤー」というクラスは、「位置を持っている」「移動できる」「ジャンプできる」「描画される」といった、プレイヤーが持つべき性質や能力を定義したもの。
    *   `Player.cs` ファイルに書かれている `internal class Player { ... }` がクラスの定義だよ。

*   **オブジェクト (Object):**
    *   クラス（設計図）に基づいて作られた**実体**。
    *   `Game.cs` の中で `Player = new Player(new Vector2(100f, 100f));` というコードがあるね。これは、「`Player` クラスの設計図を使って、実際に動かせるプレイヤーの実体を一つ作り、初期位置を (100, 100) に設定する」という意味なんだ。
    *   クラスが「たい焼きの型」なら、オブジェクトは実際に焼かれた「たい焼き」だ。一つの型から、たくさんのたい焼きを作れるように、一つのクラスから複数のオブジェクトを作ることもできる（例えば、敵キャラクターをたくさん登場させる場合など）。

```csharp
// Game.cs より
internal class Game
{
    private Player Player; // Playerクラス型の変数（まだ実体はない、箱だけ用意）

    public Game()
    {
        // new Player(...) で Playerクラスからオブジェクト(実体)を作り、
        // Player変数に代入する。初期位置を(100, 100)に設定。
        Player = new Player(new Vector2(100f, 100f));
    }
    // ...
}

// Player.cs より
internal class Player // Playerクラス（設計図）の定義
{
    // プレイヤーが持つ情報や能力がここに書かれる
    // ...
    public Player(Vector2 startPosition) // これはコンストラクタ（後述）
    {
        // オブジェクトが作られるときに初期設定を行う
        _position = startPosition;
        // ... 他の初期化 ...
    }
    // ...
}
```

#### プレイヤーの情報（位置、状態、見た目）

`Player` クラスの中には、プレイヤーの状態を表すための**変数（フィールドやプロパティと呼ばれる）** がたくさん定義されているよ。

*   `_position` (Vector2型): プレイヤーの**ワールド座標** (X, Y)。これがプレイヤーの現在地。
*   `_gravity` (int型): 現在のプレイヤーにかかっている下向きの速度（重力の影響）。ジャンプ中はマイナスになる。
*   `_imageIndex` (int型): 今表示すべきプレイヤー画像の番号。アニメーションのために使う。
*   `_isJumping` (bool型): 現在ジャンプ中（空中）かどうかを示すフラグ (`true` ならジャンプ中、`false` なら地面にいる）。
*   `_isFacingRight` (bool型): プレイヤーが右を向いているかどうかを示すフラグ (`true` なら右向き)。
*   `_movement` (Vector2型): このフレームでプレイヤーが移動しようとしている距離（X方向とY方向）。

これらの変数が、プレイヤーの今の状態をすべて表しているんだ。`Update` メソッド（次の章で詳しく見るよ）でこれらの値が変化し、`Render` メソッドでその結果が画面に描画される。

**コンストラクタ (`Player(Vector2 startPosition)`)**
クラス名と同じ名前の特別なメソッド `Player(...)` は **コンストラクタ** と呼ばれる。これは `new Player(...)` でオブジェクトが作られるときに**一度だけ自動的に呼び出される**初期化処理だよ。ここでプレイヤーの初期位置や最初の状態を設定しているんだ。

#### 画面への描き方 (`Player.Render`)

`Player` クラスには、自分自身を画面に描画するための `Render` メソッドがある。`Game.cs` の `Render` メソッドから `Player.Render();` として呼び出されるよ。

```csharp
// Player.cs の Render メソッドより抜粋
public void Render()
{
    // 1. プレイヤーのスクリーン座標を計算
    // X座標: 常に画面の中央に表示されるように計算
    float screenX = SCREEN_SIZE.X / 2f - PLAYER_HALF_SIZE.X;
    // Y座標: ワールド座標をそのまま使う（カメラのY座標を引く必要がない、今回は縦スクロールしないため）
    //        ただし、画像の左上の座標を描画するので、中心座標から画像の半分の高さを引く
    float screenY = _position.Y - PLAYER_HALF_SIZE.Y;

    // 2. 表示する画像を選ぶ
    int imageHandle = -1;
    if (_imageIndex >= 0 && _imageIndex < PLAYER_IMAGES.Length)
    {
        imageHandle = PLAYER_IMAGES[_imageIndex]; // _imageIndex番目の画像ハンドルを取得
    }

    // 3. 画像を描画する
    if (imageHandle >= 0) // 有効な画像ハンドルがある場合のみ描画
    {
        DX.DrawExtendGraph(
            (int)screenX, (int)screenY,                                 // スクリーン座標 (左上)
            (int)(screenX + PLAYER_SIZE.X), (int)(screenY + PLAYER_SIZE.Y), // スクリーン座標 (右下)
            imageHandle,                                                // 描画する画像のハンドル
            DX.TRUE                                                     // 透明色を有効に
        );
    }
}
```

ここでのポイントは **スクリーン座標の計算**。

*   `screenX`: プレイヤーは常に画面の左右中央に表示されるようにしている。画面の横幅 (`SCREEN_SIZE.X`) の半分から、プレイヤー画像の横幅の半分 (`PLAYER_HALF_SIZE.X`) を引くことで、プレイヤー画像の左端のX座標を計算している。
*   `screenY`: 今回のゲームでは縦方向にはスクロールしないため、プレイヤーのワールド座標 `_position.Y` がそのまま画面上の位置に関係してくる。ただし、描画命令 `DX.DrawExtendGraph` は画像の左上の座標を指定する必要があるので、プレイヤーの中心座標 `_position.Y` から画像の高さの半分 (`PLAYER_HALF_SIZE.Y`) を引いて、左上のY座標を計算している。

そして、`_imageIndex` (どの絵を表示するかは `UpdateAnimation` で決まる、これは後の章で) に基づいて表示する画像 (`imageHandle`) を選び、`DX.DrawExtendGraph` で計算したスクリーン座標に描画しているんだ。

**ポイント:** クラスは設計図、オブジェクトは実体。プレイヤーオブジェクトは自分の位置や状態を持ち、`Render` メソッドで自分自身を画面に描画する。

#### 第4章のまとめと質問

*   クラスが設計図、オブジェクトが実体であることを学んだ。`new` を使ってオブジェクトを作ることを知った。
*   `Player` クラスが、位置 (`_position`)、速度/重力 (`_gravity`)、状態 (`_isJumping`, `_isFacingRight`)、見た目 (`_imageIndex`) などの情報を持っていることを理解した。
*   コンストラクタはオブジェクト作成時に呼ばれる初期化処理であることを学んだ。
*   `Player.Render` メソッドが、プレイヤーのワールド座標と状態に基づいて、適切な画像を適切なスクリーン座標に描画する役割を持つことを理解した。
*   プレイヤーのスクリーンX座標は画面中央に固定され、スクリーンY座標はプレイヤーのワールドY座標から計算されることを学んだ。

**質問タイム！**

1.  `Game.cs` で `Player player1 = new Player(new Vector2(50, 50));` と `Player player2 = new Player(new Vector2(200, 50));` のように書くと、何が起こるかな？
2.  `Player.Render` メソッドで、`screenX` の計算に `playerPosition.X` (プレイヤーのワールドX座標) を使っていないのはなぜかな？ (ヒント: 第3章のカメラの動き)
3.  もしプレイヤー画像のサイズ `PLAYER_SIZE` が変わったら、`Player.Render` の計算で他に影響を受ける部分はどこかな？

---

### 第5章：プレイヤーを動かす - キーボード入力

**この章の目標:**
*   プレイヤーの状態を更新する流れを理解する。
*   キーボードからの入力をプログラムで受け取る方法を学ぶ。
*   入力に応じてプレイヤーの移動方向や状態を変化させる方法を学ぶ。
*   `Vector2` を使って位置や動きを便利に扱う方法を理解する。

#### プレイヤーを更新する (`Player.Update`)

`Player.cs` には `Update` というメソッドがある。これは `Game.cs` のゲームループ内の `Update` から呼び出され、プレイヤーに関する様々な状態を更新する役割を持っているよ。

```csharp
// Player.cs より抜粋
public void Update(InputState input)
{
    // 1. 入力に基づいて動きを決める
    HandleInput(input);
    // 2. 重力を適用する
    ApplyGravity();
    // 3. 移動し、壁や床との衝突を処理する
    MoveAndCollide();
    // 4. アニメーションを更新する
    UpdateAnimation();
}
```

`Player.Update` は、1フレーム（ゲームループの1周）ごとに、プレイヤーに対して以下の処理を順番に行うんだ。
1.  `HandleInput`: 押されているキーに応じて、左右の移動やジャンプの意思を決定する。
2.  `ApplyGravity`: 重力の影響を計算し、下方向への速度を更新する。
3.  `MoveAndCollide`: 決定された移動量に基づいて実際にプレイヤーを移動させ、壁や床にぶつかったら適切に位置を調整する。
4.  `UpdateAnimation`: 移動や状態に合わせて、表示するプレイヤーの絵（スプライト）を切り替える。

今回は、この中の `HandleInput` と、それに関連する入力処理の部分を見ていこう。

#### キーボードの入力を受け取る (`Game.GetCurrentInput`, `InputState`)

プレイヤーを動かすには、まずキーボードのどのキーが押されているかを知る必要があるね。この処理は `Game.cs` の `GetCurrentInput` メソッドが担当している。

```csharp
// Game.cs より抜粋
private InputState GetCurrentInput()
{
    // DxLibの CheckHitKey を使って各キーの状態を調べる
    // CheckHitKey(キーの種類) == 1 なら、そのキーは現在押されている
    return new InputState
    {
        Left = DX.CheckHitKey(DX.KEY_INPUT_LEFT) == 1,  // ← キー
        Right = DX.CheckHitKey(DX.KEY_INPUT_RIGHT) == 1, // → キー
        Jump = DX.CheckHitKey(DX.KEY_INPUT_SPACE) == 1, // スペースキー
    };
}

// InputState 構造体の定義 (Game.cs の上部)
internal struct InputState
{
    public bool Left;   // 左キーが押されているか (true/false)
    public bool Right;  // 右キーが押されているか (true/false)
    public bool Jump;   // ジャンプキーが押されているか (true/false)
}
```

*   `DX.CheckHitKey(キーの種類)`: DxLibの命令で、指定されたキーが**現在押されているかどうか**を調べる。押されていれば `1`、押されていなければ `0` が返ってくる。
*   `InputState`: どのキーが押されているかの情報（左、右、ジャンプ）をまとめて保持するための入れ物（**構造体 Struct** という）。`bool` 型は `true` (真) か `false` (偽) のどちらかの値を持つ型だよ。
*   `GetCurrentInput` メソッドは、各キーの状態を調べて、その結果を詰めた `InputState` オブジェクトを返す。

この `InputState` オブジェクトが、`Game.Update` から `Player.Update` へと渡されるんだ。

#### 入力に合わせて動きを決める (`Player.HandleInput`)

`Player.Update` から呼び出される `HandleInput` メソッドは、受け取った `InputState` （どのキーが押されているかの情報）に基づいて、プレイヤーの**移動の意思**を決定する。

```csharp
// Player.cs より抜粋
private Vector2 _movement; // このフレームでの移動量 (X, Y) を保持する変数

private void HandleInput(InputState input)
{
    // フレーム開始時に、まず左右の移動量をリセット
    _movement.X = 0;

    if (input.Left) // もし左キーが押されていたら
    {
        _movement.X -= PLAYER_SPEED; // 左方向（Xマイナス方向）に移動量を設定
        _isFacingRight = false;     // 左向きにする
    }
    if (input.Right) // もし右キーが押されていたら
    {
        _movement.X += PLAYER_SPEED; // 右方向（Xプラス方向）に移動量を設定
        _isFacingRight = true;      // 右向きにする
    }

    // ジャンプキーが押されていて、かつ地面にいる場合
    if (input.Jump && !_isJumping) // !_isJumping は「ジャンプ中でないなら」という意味
    {
        Jump(); // ジャンプ処理を呼び出す (これは次の章で)
    }
}
```

*   `_movement`: プレイヤーが**このフレームでどれだけ移動するか**を示す `Vector2` 型の変数。`_movement.X` が左右の移動量、`_movement.Y` が上下の移動量になる。
*   `_movement.X = 0;`: 毎フレーム最初に左右の移動量をリセットしている。こうしないと、キーを離してもプレイヤーが動き続けてしまうからね。
*   `input.Left` / `input.Right`: 渡されてきた `InputState` の情報を見て、左キーや右キーが押されているかをチェック (`true` か `false` か)。
*   `_movement.X -= PLAYER_SPEED;` / `_movement.X += PLAYER_SPEED;`: キーが押されていたら、`Constants.PLAYER_SPEED` で決められた速度分だけ、移動量 `_movement.X` を増減させる。
*   `_isFacingRight = false;` / `_isFacingRight = true;`: 移動方向に応じて、プレイヤーの向きを示すフラグ `_isFacingRight` を更新している。これは後のアニメーション処理で使う。
*   `input.Jump && !_isJumping`: ジャンプキーが押されていて、かつ `_isJumping` が `false` (つまり地面にいる) 場合にのみ、`Jump()` メソッドを呼び出す。これにより、空中での連続ジャンプを防いでいる。

**ポイント:** `HandleInput` は、キー入力の状態を見て、プレイヤーが**次にどう動きたいか** (`_movement`) と、**どちらを向くか** (`_isFacingRight`) を決定する場所。

#### `Vector2`：位置や動きをまとめて扱う便利な道具

コードの中で `Vector2` という型がよく出てくるね。これは `System.Numerics` という名前空間にある型で、2つの数値（通常は X と Y）をペアで扱うためのものだ。

```csharp
// Vector2 の使い方イメージ
Vector2 playerPosition = new Vector2(100f, 200f); // X=100, Y=200 の位置
Vector2 moveAmount = new Vector2(3f, 0f);        // X方向に3, Y方向に0 移動する量

// 位置を移動量だけ動かす
playerPosition = playerPosition + moveAmount; // playerPosition は (103f, 200f) になる

// 各要素へのアクセス
float x = playerPosition.X; // x は 103f
float y = playerPosition.Y; // y は 200f
```

`Vector2` を使うと、
*   位置 (`_position`) や移動量 (`_movement`) のように、XとYのペアで意味を持つ値をまとめて扱える。
*   ベクトル同士の足し算 (`+`) や引き算 (`-`)、定数倍 (`*`, `/`) などが簡単にできる。例えば、`_position += _movement;` のように書くだけで、X座標とY座標の両方を一度に更新できる（これは `MoveAndCollide` で使われる）。

`Player.cs` では、プレイヤーのワールド座標 `_position` と、フレームごとの移動量 `_movement` を `Vector2` で管理しているんだ。

#### 第5章のまとめと質問

*   `Player.Update` が、入力処理、重力計算、移動と衝突判定、アニメーション更新という一連の流れを実行することを学んだ。
*   `Game.GetCurrentInput` が `DX.CheckHitKey` を使ってキーボードの状態を調べ、`InputState` オブジェクトにまとめることを知った。
*   `Player.HandleInput` が `InputState` を受け取り、プレイヤーの移動量 (`_movement.X`) と向き (`_isFacingRight`) を決定することを理解した。
*   ジャンプは地面にいるときだけできるように `!_isJumping` でチェックしていることを学んだ。
*   `Vector2` がXとYの値をペアで扱い、位置や移動量の計算を簡単にする便利な型であることを知った。

**質問タイム！**

1.  もし `HandleInput` の最初の `_movement.X = 0;` を消してしまうと、プレイヤーの左右の動きはどうなると思う？
2.  `InputState` 構造体に `public bool Dash;` という項目を追加し、`GetCurrentInput` で Shift キー (`DX.KEY_INPUT_LSHIFT`) が押されたら `Dash = true;` となるようにするには、どうすればいいかな？
3.  `HandleInput` の中で、左右両方のキーが同時に押された場合、`_movement.X` の最終的な値はどうなるかな？ プレイヤーはその場で止まる？ それともどちらかに動く？

---

### 第6章：ジャンプと重力 - 物理法則の再現

**この章の目標:**
*   ゲーム内で重力をどのように表現しているか理解する。
*   ジャンプの仕組みを理解する。
*   速度と位置の関係性を理解する。

#### 重力を再現する (`Player.ApplyGravity`)

現実世界では、物は常に地球に引っ張られて下に落ちるよね。これをゲームの中で再現するのが**重力 (Gravity)** の処理だ。`Player.cs` では `ApplyGravity` メソッドがその役割を担っている。

```csharp
// Player.cs より抜粋
private int _gravity; // プレイヤーの現在の垂直方向の速度を表す変数

private void ApplyGravity()
{
    // 毎フレーム、下向きの速度(_gravity)に重力加速度(GRAVITY_INCREMENT)を加える
    _gravity += GRAVITY_INCREMENT;

    // 計算された現在の垂直速度を、このフレームでのY方向の移動量(_movement.Y)に設定する
    _movement.Y = _gravity;
}
```

ここで重要なのが `_gravity` という変数。これはプレイヤーの**垂直方向（上下方向）の速度**を表しているんだ。

*   `_gravity` がプラスの値なら下向きに移動中、マイナスの値なら上向きに移動中、0なら静止（または水平移動中）を意味する。
*   `_gravity += GRAVITY_INCREMENT;`: 毎フレーム、`_gravity` の値に `Constants.GRAVITY_INCREMENT` (定数で決めた重力の強さ、例えば 1) を足し込んでいる。これにより、
    *   落下中はどんどん下向きの速度が増していく（加速する）。
    *   ジャンプして上昇中でも、上向きの速度が徐々に減っていき、やがて下向きに転じる。
*   `_movement.Y = _gravity;`: 計算された現在の垂直速度 `_gravity` を、このフレームで実際にY方向にどれだけ移動するかを示す `_movement.Y` に設定している。

つまり、重力とは「**下向きの速度を常に少しずつ増やしていく力**」として表現されているんだね。

#### ジャンプ！ (`Player.Jump`)

ジャンプは、プレイヤーに一時的に**上向きの強い力**を与えることで実現する。`HandleInput` でジャンプキーが押されたときに呼び出される `Jump` メソッドを見てみよう。

```csharp
// Player.cs より抜粋
private void Jump()
{
    // ジャンプ状態フラグを true にする
    _isJumping = true;

    // 垂直速度(_gravity)に、上向きの力(JUMP_POWER)を設定する
    // JUMP_POWER はマイナスの値なので、上向きの速度になる
    _gravity = JUMP_POWER;

    // 注意: ここでは _movement.Y は直接変更しない。
    // ApplyGravity で _gravity の値が _movement.Y に設定される。
}
```

*   `_isJumping = true;`: まず、プレイヤーが空中（ジャンプ中）であることを示すフラグを立てる。これにより、連続ジャンプを防いだり、空中にいるときのアニメーションに切り替えたりできる。
*   `_gravity = JUMP_POWER;`: プレイヤーの垂直速度 `_gravity` に、`Constants.JUMP_POWER` で決められた値を**代入**する。`JUMP_POWER` は通常マイナスの値（例えば -20）で設定されている。なぜマイナスかというと、ゲームの座標系ではY軸は下向きがプラスだから、上向きの力はマイナスになるんだ。
    *   これにより、次の `ApplyGravity` が呼ばれるまでの間、プレイヤーは強い上向きの速度を持つことになる。

**ポイント:** ジャンプは、垂直速度 `_gravity` に瞬間的に大きなマイナス値（上向きの力）を与えることで実現される。

#### 速度と位置の関係

ここで、「速度」と「位置」の関係を整理しておこう。

*   **速度 (Velocity):** 物がどれくらいの速さで、どちらの方向に動いているかを示す量。今回のコードでは、`_gravity` が垂直方向の速度、`_movement.X` が（一時的な）水平方向の速度を表している。
*   **位置 (Position):** 物がゲーム世界のどこにいるかを示す座標。`_position` (Vector2型) がこれにあたる。

ゲームループの1フレームごとに、
1.  `HandleInput` で水平方向の**速度**（または移動量）`_movement.X` が決まる。
2.  `ApplyGravity` で重力の影響を受けて垂直方向の**速度** `_gravity` が更新され、それが `_movement.Y` に設定される。
3.  `MoveAndCollide` (次の章で詳しく見る) で、そのフレームでの移動量 `_movement` (XとY) を使って、プレイヤーの**位置** `_position` が更新される (`_position += _movement;` のような計算が行われる)。

**イメージ:**
車を運転するとき、アクセルを踏むと**速度**が上がる。その速度で一定時間走ると、車の**位置**が進む。ゲームのキャラクターも同じで、フレームごとに速度を計算し、その速度に基づいて位置を更新していくことで動きを表現しているんだ。

```
 [フレーム1]
 HandleInput:   _movement.X = 3 (右移動)
 ApplyGravity:  _gravity = 1, _movement.Y = 1 (少し落下)
 MoveAndCollide: _position が (3, 1) だけ変化

 [フレーム2]
 HandleInput:   _movement.X = 3 (右移動)
 ApplyGravity:  _gravity = 2, _movement.Y = 2 (もう少し落下)
 MoveAndCollide: _position が (3, 2) だけ変化

 [フレーム3] (ジャンプキーが押された！)
 HandleInput:   _movement.X = 3, Jump() が呼ばれる -> _gravity = -20
 ApplyGravity:  _gravity = -19, _movement.Y = -19 (強く上昇！)
 MoveAndCollide: _position が (3, -19) だけ変化
```
*図6-1: フレームごとの速度と位置の変化の例*

#### 第6章のまとめと質問

*   重力は、毎フレームプレイヤーの下向き速度 (`_gravity`) を少しずつ増加させることで表現されている (`ApplyGravity`)。
*   ジャンプは、プレイヤーの垂直速度 (`_gravity`) に瞬間的に強い上向きの力 (`JUMP_POWER`、マイナス値) を与えることで実現されている (`Jump`)。
*   `_gravity` は垂直方向の速度を表し、`_movement.Y` はその速度に基づいて計算されるフレームごとのY方向移動量である。
*   速度に基づいてフレームごとに位置を更新していくことで、キャラクターの動きが表現されることを学んだ。

**質問タイム！**

1.  もし `Constants.GRAVITY_INCREMENT` の値を大きく（例えば 3 に）したら、ゲームはどう変わるかな？ 逆に小さく（例えば 0.5、ただし `int` なので工夫が必要）したら？
2.  `Jump` メソッドの中で `_gravity = JUMP_POWER;` の代わりに `_gravity += JUMP_POWER;` と書いたら、ジャンプの挙動はどう変わってしまうだろう？
3.  プレイヤーが地面にいるとき（`_isJumping` が `false` のとき）、`_gravity` の値はどうなっているべきかな？ (ヒント: 次の章の衝突判定が関係する)

---

### 第7章：壁と床 - 衝突判定（基本編）

**この章の目標:**
*   なぜ衝突判定が必要なのか理解する。
*   プレイヤーとタイルマップの当たり判定の基本的な考え方を学ぶ。
*   横方向（壁）の衝突を検出し、プレイヤーの位置を調整する方法を理解する。

#### なぜ衝突判定が必要？

これまでの処理だけだと、プレイヤーは重力で下に落ち続け、左右のキーを押せば無限に移動できてしまう。現実のゲームでは、プレイヤーは地面の上を歩き、壁があればそれ以上進めないよね。

これを実現するのが**衝突判定 (Collision Detection)** と**衝突応答 (Collision Response)** だ。

*   **衝突判定:** プレイヤーが壁や床などの障害物と接触しているかどうかを調べる処理。
*   **衝突応答:** 衝突が検出された場合に、プレイヤーが障害物にめり込まないように位置を調整したり、速度を変えたりする処理。

`Player.cs` の `MoveAndCollide` メソッドの中で、これらの処理が行われている。今回はまず、横方向（壁との）衝突を見ていこう。

#### タイルとの当たり判定 (`Constants.IsSolidTile`)

プレイヤーが衝突するかどうかを判断するには、移動先のタイルが「通れる場所（空）」なのか「通れない場所（壁や床）」なのかを知る必要がある。この判断をしてくれるのが `Constants.cs` にある `IsSolidTile` メソッドだ。

```csharp
// Constants.cs より抜粋
public static bool IsSolidTile(int tileX, int tileY)
{
    // マップの範囲外は壁扱い (マップから落ちないように)
    if (tileX < 0 || tileX >= MAP_WIDTH || tileY < 0 || tileY >= MAP_HEIGHT)
    {
        return true; // true = 固いタイル (通れない)
    }
    // マップデータ(MAP_DATA)の [tileY][tileX] の値が 0 でなければ固いタイル
    // (0 は空白タイルとみなす、というルール)
    return MAP_DATA[tileY][tileX] != 0; // 0でなければ true (固い), 0なら false (空白)
}
```

このメソッドは、指定されたタイル座標 `(tileX, tileY)` が、
*   マップの範囲外か？ (範囲外なら壁扱い)
*   マップデータ上で 0 (空白タイル) 以外の番号か？
をチェックし、通れない固いタイルなら `true`、通れる空白タイルなら `false` を返す。

**ポイント:** `IsSolidTile` は、特定の座標のタイルが障害物かどうかを教えてくれる便利な関数。

#### 横方向の衝突 (`Player.ResolveCollisionHorizontal`)

`MoveAndCollide` メソッドは、まず `_movement.X` の分だけプレイヤーのX座標を更新し、その後すぐに `ResolveCollisionHorizontal` を呼び出して壁との衝突をチェック・解決する。

```csharp
// Player.cs の MoveAndCollide メソッドの一部
private void MoveAndCollide()
{
    // --- 横方向の移動と衝突判定 ---
    _position.X += _movement.X;   // まずX方向に移動してみる
    ResolveCollisionHorizontal(); // 壁にめり込んでないかチェック＆修正

    // --- 縦方向の移動と衝突判定 --- (次の章で詳しく)
    _position.Y += _movement.Y;
    ResolveCollisionVertical();
}

// Player.cs より抜粋
private void ResolveCollisionHorizontal()
{
    // プレイヤーの当たり判定の範囲を計算
    float halfWidth = PLAYER_HALF_SIZE.X; // 横幅の半分
    float halfHeight = PLAYER_HALF_SIZE.Y; // 縦幅の半分

    // チェックするタイルのY座標の範囲を決める (プレイヤーの身長分)
    int topTile = (int)Math.Floor((_position.Y - halfHeight) / GROUND_SIZE);
    int bottomTile = (int)Math.Floor((_position.Y + halfHeight - 0.001f) / GROUND_SIZE); // 微小な値を引いて境界バグを防ぐ
    // 範囲をマップ内に収める
    topTile = Math.Max(0, topTile);
    bottomTile = Math.Min(MAP_HEIGHT - 1, bottomTile);

    // 左方向への移動(_movement.X < 0)の場合
    if (_movement.X < 0)
    {
        // プレイヤーの左端が接触する可能性のあるタイルのX座標
        int leftTileX = (int)Math.Floor((_position.X - halfWidth) / GROUND_SIZE);

        // プレイヤーの身長分の範囲で、左側のタイルをチェック
        for (int y = topTile; y <= bottomTile; y++)
        {
            if (Constants.IsSolidTile(leftTileX, y)) // もし壁タイルだったら
            {
                // 衝突応答！
                // プレイヤーの左端を、壁タイルの右端にピッタリ合わせる
                _position.X = (leftTileX + 1) * GROUND_SIZE + halfWidth;
                _movement.X = 0; // 横方向の移動量をゼロにする
                return;          // 衝突が見つかったら処理終了
            }
        }
    }
    // 右方向への移動(_movement.X > 0)の場合
    else if (_movement.X > 0)
    {
        // プレイヤーの右端が接触する可能性のあるタイルのX座標
        int rightTileX = (int)Math.Floor((_position.X + halfWidth - 0.001f) / GROUND_SIZE);

        // プレイヤーの身長分の範囲で、右側のタイルをチェック
        for (int y = topTile; y <= bottomTile; y++)
        {
            if (Constants.IsSolidTile(rightTileX, y)) // もし壁タイルだったら
            {
                // 衝突応答！
                // プレイヤーの右端を、壁タイルの左端にピッタリ合わせる
                _position.X = rightTileX * GROUND_SIZE - halfWidth;
                _movement.X = 0; // 横方向の移動量をゼロにする
                return;          // 衝突が見つかったら処理終了
            }
        }
    }
}
```

処理の流れを追いかけてみよう。

1.  **当たり判定の範囲計算:** プレイヤーの中心座標 (`_position`) とサイズ (`PLAYER_HALF_SIZE`) から、プレイヤーが存在する矩形（四角い範囲、**バウンディングボックス**と呼ばれる）を考える。
2.  **チェック対象タイルの特定:**
    *   まず、プレイヤーの身長に相当するY座標のタイル範囲 (`topTile` から `bottomTile` まで) を特定する。壁に当たるかどうかは、頭から足元までのどこかが当たれば良いからね。
    *   次に、移動方向に応じて、接触する可能性のあるX座標のタイル (`leftTileX` または `rightTileX`) を特定する。
        *   左移動中 (`_movement.X < 0`) なら、プレイヤーの**左端**がどのX座標のタイルに接触するか (`leftTileX`) を計算。
        *   右移動中 (`_movement.X > 0`) なら、プレイヤーの**右端**がどのX座標のタイルに接触するか (`rightTileX`) を計算。
        *   ここでも `Math.Floor` と `GROUND_SIZE` を使って、ピクセル座標からタイル座標へ変換している。
3.  **衝突判定:**
    *   特定したタイル座標 (`leftTileX` または `rightTileX`) と、Y座標の範囲 (`topTile` から `bottomTile`) を使って、`for` ループで縦に並んだタイルを一つずつチェック。
    *   `Constants.IsSolidTile(タイルX, タイルY)` を呼び出し、そのタイルが固い壁かどうかを調べる。
4.  **衝突応答:**
    *   もし固いタイル (`IsSolidTile` が `true`) が見つかったら、衝突発生！
    *   プレイヤーが壁にめり込まないように、**位置を調整**する。
        *   左に衝突した場合: プレイヤーの**左端** (`_position.X - halfWidth`) が、衝突した壁タイル(`leftTileX`)の**右端** (`(leftTileX + 1) * GROUND_SIZE`) にピッタリ合うように `_position.X` を再設定する。
        *   右に衝突した場合: プレイヤーの**右端** (`_position.X + halfWidth`) が、衝突した壁タイル(`rightTileX`)の**左端** (`rightTileX * GROUND_SIZE`) にピッタリ合うように `_position.X` を再設定する。
    *   同時に、壁にぶつかったので、それ以上横に進めないように `_movement.X = 0;` として、横方向の移動量をゼロにする。
    *   `return;` で、衝突処理を終了する（一つの壁にぶつかれば十分なので）。

![](./placeholder_images/collision_h.png)
*図7-1: 横方向の衝突判定と応答のイメージ (右移動時)*

**ポイント:** まず移動してみて、その後、移動先のタイルが壁かどうかをチェックし、壁だったらめり込まないように位置を押し戻すのが基本的な流れ。

#### 第7章のまとめと質問

*   プレイヤーが地面を歩いたり壁で止まったりするためには、衝突判定と応答が必要であることを学んだ。
*   `Constants.IsSolidTile` が、指定された座標のタイルが通れない固いタイルかどうかを教えてくれる関数であることを理解した。
*   横方向の衝突判定 (`ResolveCollisionHorizontal`) では、プレイヤーの移動先の左右の端が壁タイルに接触していないかをチェックすることを学んだ。
*   衝突が検出された場合、プレイヤーの位置を壁タイルの端にピッタリ合わせるように調整し、横方向の移動量をゼロにするという衝突応答処理を理解した。

**質問タイム！**

1.  `ResolveCollisionHorizontal` の中で、なぜプレイヤーの身長分のY座標範囲 (`topTile` から `bottomTile` まで) をチェックする必要があるのかな？ 一番下のタイルだけチェックすれば十分？
2.  もし `ResolveCollisionHorizontal` の最後の `_movement.X = 0;` をコメントアウトしたら、プレイヤーが壁にぶつかったとき、どのような動きになると思う？
3.  プレイヤーの幅 (`PLAYER_SIZE.X`) を今の2倍にしたら、`ResolveCollisionHorizontal` の計算で修正が必要な箇所はあるかな？

---

### 第8章：壁と床 - 衝突判定（応用編）

**この章の目標:**
*   縦方向（床や天井）の衝突を検出し、プレイヤーの位置や状態を調整する方法を理解する。
*   地面に着地したときの処理（ジャンプ状態の解除、重力リセット）を学ぶ。
*   マップから落下した場合の処理を考える。

#### 縦方向の衝突 (`Player.ResolveCollisionVertical`)

横方向と同じように、`MoveAndCollide` メソッドでは `_movement.Y` の分だけプレイヤーのY座標を更新した後、`ResolveCollisionVertical` を呼び出して床や天井との衝突をチェック・解決する。

```csharp
// Player.cs より抜粋
private void ResolveCollisionVertical()
{
    // プレイヤーの当たり判定の範囲を計算
    float halfWidth = PLAYER_HALF_SIZE.X;
    float halfHeight = PLAYER_HALF_SIZE.Y;

    // チェックするタイルのX座標の範囲を決める (プレイヤーの横幅分)
    int leftTileX = (int)Math.Floor((_position.X - halfWidth) / GROUND_SIZE);
    int rightTileX = (int)Math.Floor((_position.X + halfWidth - 0.001f) / GROUND_SIZE);
    // 範囲をマップ内に収める
    leftTileX = Math.Max(0, leftTileX);
    rightTileX = Math.Min(MAP_WIDTH - 1, rightTileX);

    // 下方向への移動 (_movement.Y > 0、つまり落下中か着地しようとしている時)
    if (_movement.Y > 0)
    {
        // プレイヤーの足元が接触する可能性のあるタイルのY座標
        int bottomTileY = (int)Math.Floor((_position.Y + halfHeight - 0.001f) / GROUND_SIZE);

        // マップの下端よりも下に行ってしまったか？ (落下)
        if (bottomTileY >= MAP_HEIGHT)
        {
            Console.WriteLine("Player fell off the map!"); // メッセージ表示
            // ★★★ ここでリスポーン処理などを行う ★★★
            _position = new Vector2(100f, 100f); // 例: 初期位置に戻す
            _gravity = 0;
            _isJumping = false;
            _movement = Vector2.Zero;
            return; // 処理終了
        }

        // プレイヤーの横幅分の範囲で、足元のタイルをチェック
        for (int x = leftTileX; x <= rightTileX; x++)
        {
            if (Constants.IsSolidTile(x, bottomTileY)) // もし床タイルだったら
            {
                // 衝突応答！ (着地)
                // プレイヤーの足元を、床タイルの上端にピッタリ合わせる
                _position.Y = bottomTileY * GROUND_SIZE - halfHeight;
                _gravity = 0;       // 垂直速度をリセット！ これ以上落ちない
                _isJumping = false; // 地面に着いたのでジャンプ状態を解除
                _movement.Y = 0;    // 縦方向の移動量をゼロにする
                return;             // 衝突が見つかったら処理終了
            }
        }
        // ループを抜けても床が見つからなかった場合 (落下中)
        _isJumping = true; // まだ空中なのでジャンプ状態にしておく
    }
    // 上方向への移動 (_movement.Y < 0、つまりジャンプ上昇中)
    else if (_movement.Y < 0)
    {
        // プレイヤーの頭上が接触する可能性のあるタイルのY座標
        int topTileY = (int)Math.Floor((_position.Y - halfHeight) / GROUND_SIZE);

        // マップの上端よりも上に行ってしまったか？ (あまりないはずだが念のため)
        if (topTileY < 0)
        {
            _position.Y = halfHeight; // 画面上端で止める
            _gravity = 0;
            _movement.Y = 0;
            return;
        }

        // プレイヤーの横幅分の範囲で、頭上のタイルをチェック
        for (int x = leftTileX; x <= rightTileX; x++)
        {
            if (Constants.IsSolidTile(x, topTileY)) // もし天井タイルだったら
            {
                // 衝突応答！ (天井に頭をぶつけた)
                // プレイヤーの頭上を、天井タイルの下端にピッタリ合わせる
                _position.Y = (topTileY + 1) * GROUND_SIZE + halfHeight;
                _gravity = 0;    // 上昇速度をリセット！ これ以上昇らない
                _movement.Y = 0; // 縦方向の移動量をゼロにする
                return;          // 衝突が見つかったら処理終了
            }
        }
    }
}
```

これも横方向と似ているけど、特に重要な点がいくつかあるよ。

#### 地面に着地したときの処理

プレイヤーが下に移動している (`_movement.Y > 0`) とき、足元のタイル (`bottomTileY`) をチェックし、もしそれが固いタイル (`IsSolidTile` が `true`) なら、**着地**したと判断する。

着地時の衝突応答は、位置調整に加えて、**物理状態の更新**が重要だ。

1.  **位置調整:** プレイヤーの足元 (`_position.Y + halfHeight`) が、着地した床タイル (`bottomTileY`) の上端 (`bottomTileY * GROUND_SIZE`) にピッタリ合うように `_position.Y` を再設定する。
2.  **重力（垂直速度）のリセット:** `_gravity = 0;` これが非常に重要！ 地面に着いたのだから、それ以上下に落ちる速度はなくなる。これをしないと、地面にめり込もうとする力が働き続けてしまう。
3.  **ジャンプ状態の解除:** `_isJumping = false;` 地面に着いたので、「ジャンプ中（空中）」フラグを `false` にする。これにより、再びジャンプキーを押せばジャンプできるようになる。
4.  **移動量の停止:** `_movement.Y = 0;` これ以上Y方向に移動しないようにする。

![](./placeholder_images/collision_v_ground.png)
*図8-1: 地面への着地処理のイメージ*

**もし落下中に足元に床が見つからなかったら？**
`for` ループで足元のタイルをチェックしても固いタイルが見つからなかった場合、プレイヤーはまだ空中にいる（落下中）ということ。その場合は `_isJumping = true;` として、空中状態を維持する。

#### 天井に頭をぶつけたときの処理

プレイヤーが上に移動している (`_movement.Y < 0`) とき、頭上のタイル (`topTileY`) をチェックし、もしそれが固いタイルなら、**天井に頭をぶつけた**と判断する。

この場合も、位置調整と物理状態の更新を行う。

1.  **位置調整:** プレイヤーの頭 (`_position.Y - halfHeight`) が、ぶつかった天井タイル (`topTileY`) の下端 (`(topTileY + 1) * GROUND_SIZE`) にピッタリ合うように `_position.Y` を再設定する。
2.  **重力（垂直速度）のリセット:** `_gravity = 0;` 天井にぶつかったので、それ以上の上昇速度はなくなる。これをしないと、天井にめり込もうとする力が働き続ける。
3.  **移動量の停止:** `_movement.Y = 0;` これ以上Y方向に移動しないようにする。

![](./placeholder_images/collision_v_ceil.png)
*図8-2: 天井への衝突処理のイメージ*

#### マップから落ちたら？

`ResolveCollisionVertical` の下方向チェックの最初には、プレイヤーの足元がマップの範囲外 (`bottomTileY >= MAP_HEIGHT`) に出てしまった場合の処理がある。

```csharp
if (bottomTileY >= MAP_HEIGHT)
{
    Console.WriteLine("Player fell off the map!");
    // ★★★ ここでリスポーン処理などを行う ★★★
    _position = new Vector2(100f, 100f); // 例: 初期位置に戻す
    _gravity = 0;
    _isJumping = false;
    _movement = Vector2.Zero;
    return;
}
```
ここでは、メッセージをコンソールに出力した後、プレイヤーの位置を初期位置 (100, 100) に戻し、速度やジャンプ状態もリセットしている。実際のゲームでは、残機を減らしたり、ゲームオーバー画面を表示したりといった処理を追加することになるだろう。

**ポイント:** 縦方向の衝突判定では、床への着地時に `_gravity` のリセットと `_isJumping` の更新が特に重要。

#### 第8章のまとめと質問

*   縦方向の衝突判定 (`ResolveCollisionVertical`) では、プレイヤーの足元（落下/着地時）や頭上（ジャンプ上昇時）が床や天井タイルに接触していないかをチェックすることを学んだ。
*   地面に着地した場合、位置を調整するだけでなく、垂直速度 (`_gravity`) をゼロにし、ジャンプ状態 (`_isJumping`) を `false` にすることが重要だと理解した。
*   天井に頭をぶつけた場合も、位置を調整し、垂直速度 (`_gravity`) をゼロにする必要があることを学んだ。
*   マップから落下した場合の特別な処理（リスポーンなど）を追加できることを知った。

**質問タイム！**

1.  もし着地時に `_gravity = 0;` をしなかったら、プレイヤーはどうなってしまうだろう？ （ヒント: `ApplyGravity` は毎フレーム呼ばれる）
2.  もし着地時に `_isJumping = false;` をしなかったら、プレイヤーはどうなってしまうだろう？ (ヒント: `HandleInput` のジャンプ条件)
3.  現在のコードでは、プレイヤーが左右の壁に接触したままジャンプすると、壁を登れてしまう可能性がある（壁キックのような状態）。これを防ぐには、どのようなチェックを追加すれば良いかアイデアはあるかな？

---

### 第9章：プレイヤーを生き生きと - アニメーション

**この章の目標:**
*   プレイヤーの状態に合わせて見た目を変えるアニメーションの仕組みを理解する。
*   時間経過で画像を切り替える基本的な方法を学ぶ。
*   ゲーム全体を振り返り、さらに発展させるためのアイデアを得る。

#### アニメーションの仕組み (`Player.UpdateAnimation`)

ゲームのキャラクターがただ動くだけでなく、歩いたりジャンプしたりしているように見えるのは、**アニメーション (Animation)** のおかげだね。これは、パラパラ漫画のように、少しずつ違う絵（スプライト）を順番に表示することで動きを表現する技術だよ。

`Player.cs` の `UpdateAnimation` メソッドが、プレイヤーのアニメーションを制御している。このメソッドは `Player.Update` の最後に呼び出される。

```csharp
// Player.cs より抜粋
private int _imageIndex;       // 現在表示する画像のインデックス番号
private int _imageIndexCount;  // アニメーションの速度を調整するためのカウンター
private bool _isFacingRight;   // 右向きか左向きか

private void UpdateAnimation()
{
    const int animSpeed = 10; // アニメーションの速度（このフレーム数ごとに絵を切り替える）

    // 1. ジャンプ中のアニメーション
    if (_isJumping) // もしジャンプ中(空中)なら
    {
        // 右向きなら画像1番、左向きなら画像4番を使う (0から数える)
        _imageIndex = _isFacingRight ? 1 : 4;
        _imageIndexCount = 0; // カウンターリセット
    }
    // 2. 歩いているときのアニメーション
    else if (_movement.X != 0) // もし地面にいて、かつ左右に移動中なら
    {
        _imageIndexCount++; // カウンターを増やす
        // カウンターが一定値を超えたらループさせる (2枚のアニメでループ)
        if (_imageIndexCount >= animSpeed * 2)
        {
            _imageIndexCount = 0;
        }

        // カウンターの値に応じて表示する絵を切り替える
        if (_imageIndexCount < animSpeed)
        {
            // 歩きアニメ1枚目 (右向き:1番, 左向き:4番)
            _imageIndex = _isFacingRight ? 1 : 4;
        }
        else
        {
            // 歩きアニメ2枚目 (右向き:2番, 左向き:5番)
            _imageIndex = _isFacingRight ? 2 : 5;
        }
    }
    // 3. 止まっているとき（アイドル状態）のアニメーション
    else // 地面にいて、移動していない場合
    {
        // 待機アニメ (右向き:0番, 左向き:3番)
        _imageIndex = _isFacingRight ? 0 : 3;
        _imageIndexCount = 0; // カウンターリセット
    }
}
```

このコードは、プレイヤーの**現在の状態** (`_isJumping`, `_movement.X`, `_isFacingRight`) に応じて、表示すべき画像の番号 (`_imageIndex`) を決定している。

*   **状態の判定:** `if` や `else if` を使って、プレイヤーが「ジャンプ中か？」「歩いているか？」「止まっているか？」を判断する。
*   **向きの判定:** `_isFacingRight ? A : B` という書き方（三項演算子）は、「もし `_isFacingRight` が `true` なら A を、`false` なら B を使う」という意味。これで左右の向きに合わせた画像を選んでいる。
    *   `PLAYER_IMAGES` 配列には、おそらく [右向き待機, 右向き歩き1, 右向き歩き2, 左向き待機, 左向き歩き1, 左向き歩き2] のような順番で画像ハンドルが格納されていると想定される（`LoadSprites` の引数から推測）。
*   **表示画像の決定:** 状態と向きに応じて、`_imageIndex` に適切な画像の番号を設定する。この `_imageIndex` が、`Player.Render` で実際に描画する画像を選ぶのに使われる。

#### パラパラ漫画の原理

歩きアニメーションの部分では、`_imageIndexCount` というカウンターと `animSpeed` という定数を使って、パラパラ漫画のような効果を出している。

1.  プレイヤーが歩き始めると (`_movement.X != 0`)、`_imageIndexCount` がフレームごとに 1 ずつ増えていく。
2.  `_imageIndexCount` が `animSpeed` (例えば 10) 未満の間は、歩きアニメの1枚目 (`_imageIndex = 1` or `4`) を表示する。
3.  `_imageIndexCount` が `animSpeed` 以上 `animSpeed * 2` 未満の間は、歩きアニメの2枚目 (`_imageIndex = 2` or `5`) を表示する。
4.  `_imageIndexCount` が `animSpeed * 2` 以上になったら、カウンターを 0 にリセットし、再び1枚目のアニメーションから繰り返す。

これにより、`animSpeed` フレームごとに歩く絵が切り替わり、歩いているように見えるんだ。`animSpeed` の値を変えれば、アニメーションの速度を調整できる。

**ポイント:** アニメーションは、プレイヤーの状態に応じて表示する画像を切り替え、時間経過で画像を変化させることで実現する。

#### 第9章のまとめと最終課題

*   `UpdateAnimation` メソッドが、プレイヤーの状態（ジャンプ中、歩行中、待機中）と向き（右向き、左向き）に応じて、表示する画像のインデックス (`_imageIndex`) を決定することを学んだ。
*   `_imageIndexCount` と `animSpeed` を使って、一定時間ごとに画像を切り替えることで、歩行アニメーションのような動きを表現する仕組みを理解した。
*   三項演算子 (`条件 ? 値1 : 値2`) が、条件によって値を切り替えるのに便利なことを知った。

**講座のまとめ**
この講座では、C#とDxLibを使って、シンプルな横スクロールアクションゲームの基本的な要素（ゲームループ、定数管理、アセット読み込み、座標系、スクロール、プレイヤー表示、入力処理、物理演算、衝突判定、アニメーション）を一つずつ学んできました。

**最終課題（アイデア）**
ここまでの知識を基に、このゲームをさらに面白くしてみよう！

1.  **新しいアクションの追加:**
    *   Shiftキーでダッシュできるようにしてみよう (`InputState` に項目を追加し、`HandleInput` で `PLAYER_SPEED` を一時的に上げる)。
    *   特定のキーで攻撃アニメーションを表示できるようにしてみよう (`UpdateAnimation` に状態を追加)。
2.  **アイテムの追加:**
    *   マップ上にコインなどのアイテムを配置し、プレイヤーが取れるようにしてみよう（アイテム用のクラスを作り、衝突判定を追加）。
    *   取ったコインの数を画面に表示してみよう (`DX.DrawString`)。
3.  **敵キャラクターの追加:**
    *   簡単な動き（左右往復など）をする敵キャラクタークラスを作ってみよう。
    *   プレイヤーが敵に当たったら、リスポーンするようにしてみよう。
    *   （応用）プレイヤーが敵を踏んだら倒せるようにしてみよう（衝突判定の応用）。
4.  **マップの拡張:**
    *   `tilemap.csv` を編集して、もっと複雑なステージを作ってみよう。
    *   特定のタイルを踏むとジャンプ力が上がる、などのギミックを追加してみよう。

これらの課題に挑戦することで、さらにプログラミングとゲーム開発の理解が深まるはずだよ。頑張って！
