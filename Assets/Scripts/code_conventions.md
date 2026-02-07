# コード規約
## 概要
- 本プロジェクトにおけるコード規約を記載している
- 基本的にこのファイルに則ってコードを書くこと
- コード規約に書いておらず、わからない場合は質問してください

# 基本構成
- 基本的にModel・View・ControllerのMVCパターンを採用していく

## Model
- 対象のモデルのルールを記述する
- MonoBehaviourを継承しない
- ビジネスロジックをカプセル化する
- ビジネスロジックに必要なインターフェースはここに配置する

## View
- MonoBehaviourを継承し、UnityのGameObjectと紐づく
- ModelとControllerをDIで注入する
- ライフサイクル処理は行わない
- Controllerからの更新をGameObjectに反映するのみ
  
## Controller
- MonoBehaviourを継承しない純粋なクラス
- Modelの計算結果を受け取る
- ビジネスロジック処理に特化
- DIで View に注入される
- テスト性が高い
- IStartable、ITickableなどのVContainerライフサイクルインターフェースを実装してライフサイクル処理を行う

## Service
- Modelだけでは実現できないロジックの記述に使う
- Modelだけでビジネスロジックが完結する場合は使わなくてもよい
- Serviceクラスの追加を検討する場合
  - 複数のModelを組み合わせる
  - 外部APIをたたく
  - DB操作をする など
- Controllerが依存する

# 命名規則
## ファイル名・クラス名
- アッパーキャメルケース

# 各メソッドの書き方
## コンストラクタ
```
public ExampleController(
    ExampleModel model,
    ExampleView view)
{
    this.model = model; // thisは省略しない
    this.view = view; // ← パラメータと変数が同名の時、this で区別
}
```

## SerializeField
```csharp
[SerializeField] private GameObject hoge; // privateは省略しない。アンダースコアもつけない
```

## public メソッド
```csharp
public void UpdatePosition(Vector3 newPosition)
{
    if (hoge) {
        // 処理
    } else {
        // 処理
    }
}
```

## privateメソッド
```csharp
private void Hoge() // private は省略しない
{

}
```

## private変数
```csharp
private int test; // 先頭アンダースコアは省略する。privateは省略しない
private readonly Subject<Unit> attacked = new Subject<Unit>(); // readonlyも同様
```

## メソッド内変数
```
private void Hoge()
{
    var test = 1; // 極力varを使う
}
```

## クラス定義
```
public sealed class Hoge
{
    // 可能であれば sealed を付ける
}
```

## クラス継承
- 極力禁止
- ポリモーフィズムはインターフェースで実現する
- コードの共通化はクラスを分けるなどで実現する

## プロパティ
```csharp
// シンプルな変数はプロパティにする
public int Hp { get; set; }

// 読み取り専用プロパティは => 構文を使う。{ get; private set; }は使わない
public Vector3 Position => position;

// R3などのリアクティブなプロパティは、実態をprivateでもち、IObservableのみを公開する
public IObservable<Unit> Attacked => attacked;
private readonly Subject<Unit> attacked = new Subject<Unit>();
```

## Dependency Injection（VContainer）

### コンストラクタインジェクション（推奨）
```csharp
public sealed class OrbitCameraController
{
    private readonly OrbitCameraModel model;
    private readonly IMouseInput mouseInput;

    public OrbitCameraController(
        OrbitCameraModel model,
        IMouseInput mouseInput,
        float horizontalSensitivity = 2f)
    {
        this.model = model;
        this.mouseInput = mouseInput;
    }
}
```

### メソッドインジェクション
```csharp
public sealed class CameraView : MonoBehaviour
{
    private OrbitCameraController controller;
    private OrbitCameraModel model;

    [Inject]
    private void Construct(OrbitCameraController controller, OrbitCameraModel model)
    {
        this.controller = controller;
        this.model = model;
    }
}
```

## Lifecycleメソッドの記述順序

VContainerライフサイクルを使用する（推奨）
```csharp
public sealed class HogeController : IStartable, ITickable
{
    public void Start()
    {
        // 初期化（MonoBehaviour.Start相当）
    }

    public void Tick()
    {
        // 毎フレーム処理（MonoBehaviour.Update相当）
    }
}
```

**注記**
- MonoBehaviourライフサイクル（Awake、Update等）は使用しない
- VContainerのIStartable、ITickableを使用してライフサイクル処理を実装する
- Viewはライフサイクル処理を行わない

## LifetimeScope
- VContainerによるDI設定を行う
- 各機能ごとにLifetimeScopeを分割する
- 例：CameraLifetimeScope、PlayerLifetimeScope

```csharp
public sealed class CameraLifetimeScope : LifetimeScope
{
    [SerializeField] private CameraView cameraView;
    [SerializeField] private Transform targetTransform;

    protected override void Configure(IContainerBuilder builder)
    {
        // Model登録
        builder.Register<OrbitCameraModel>(
            _ => new OrbitCameraModel(targetTransform.position),
            Lifetime.Singleton);

        // Interface登録
        builder.Register<IMouseInput>(
            _ => new MouseInput(),
            Lifetime.Singleton);

        // Controller登録
        builder.Register<OrbitCameraController>(
            container => new OrbitCameraController(
                container.Resolve<OrbitCameraModel>(),
                container.Resolve<IMouseInput>()),
            Lifetime.Singleton);

        // View登録
        builder.RegisterComponent(cameraView);
    }
}
```

## 非同期処理
- UniTaskを使用する
- コルーチンは使用しない
