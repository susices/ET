# SJET Framework
本项目是基于[ET6.0框架](https://github.com/egametang/ET) 的个人修改版框架

项目目标是提供更完善的客户端功能以及分享在ET框架下的一些组件编写思路

## 框架层

### 资源管理

描述：

包含了按目录标记AssetBundle功能， 同个目录下的资源打进同个AB中。 编辑器下AB循环依赖分析功能。

使用方法:

按资源相对路径异步加载资源：
```csharp
GameObject assetObj = await ResourcesComponent.Instance.LoadAndGetAssetByPathAsync<GameObject>("AssetPath");
```

框架层面使用对象池管理资源实例 通过AssetEntity管理资源GameObject 

根据本地化资源ID异步获取实例化资源：
```csharp
AssetEntity assetEntity = await PoolingAssetComponent.Instance.GetAssetEntityAsync(uiAssetPathIndex);
```
根据本地化资源ID同步获取实例化资源：
```csharp
AssetEntity assetEntity = PoolingAssetComponent.Instance.GetAssetEntity(uiAssetPathIndex);
```

释放实例化资源： 

AssetEntity管理的资源只能通过这种方式释放  不可以Destroy
```csharp
assetEntity.Dispose();
```

AssetEntity资源释放后会缓存一定时间，时间过后没有被使用则销毁资源。


### UI

描述： 

通过UIPanel UIComponent UIItem等非Mono脚本构建的可复用资源和逻辑的UI框架.
支持UI生命周期, 支持UI资源的对象池, 支持UIPanel的嵌套, 支持UIPanel传入泛型参数.

相关对象:  UIPanel UIPanelComponent SubUIPanel UIItem UIComponent

UIPanel: UI页面实体类, 持有UI页面Prefab引用, 描述UIPanel基本信息. UIPanel只能挂载到UIPanelComponent.

UIPanelComponent: 持有UIPanel列表.

SubUIPanel: UIPanel的子Panel 需挂载至UIPanel中的UIPanelComponent.

UIItem: UIPanel中挂载的UI物体

UIComponent: 挂载至UIPanel的UI逻辑组件, 负责Ui业务逻辑.

使用方法：

打开UIPanel

```csharp
await args.ZoneScene.ShowUIPanel(UIPanelType.UIHUD);
```

关闭UIPanel

```csharp
self.DomainScene().RemoveUIPanel(UIPanelType.UIBag).Coroutine();
```

UIComponent编写

需要添加特性标记UIComponent 序号 

```csharp
    [UIPanelComponent(UiPanelComponentIndex.UILogin)]
    public class UILoginComponent: Entity
    {

    }
```


### 数据集组件

描述：

通用的支持数据全量更新, 差异更新, 数据更新事件监听的数据集组件. 

使用方法：

添加数据集组件  参数为数据类型枚举

```csharp
self.AddComponent<DataSetComponent,DataType>(DataType.BagItem);
```

添加数据更新监听

```csharp
DataUpdateComponent.Instance.AddListener(DataType.BagItem, self);
```

移除数据更新监听

```csharp
DataUpdateComponent.Instance.RemoveListener(DataType.BagItem, self);
```


处理数据更新事件

```csharp
    public class DataUpdateEvent: AEvent<EventType.DataUpdate>
    {
        protected override async ETTask Run(EventType.DataUpdate args)
        {
            if (!DataUpdateComponent.Instance.DataUpdateComponents.TryGetDic(args.DataType, out var dic))
            {
                return;
            }

            if (args.DataType == DataType.BagItem)
            {
                foreach (var component in dic.Values)
                {
                    if (component is UIBagComponent uiBagComponent)
                    {
                        uiBagComponent.OnDataUpdate();
                        continue;
                    }
                }
                return;
            }
            await ETTask.CompletedTask;
        }
    }
```


### 本地化

## 业务层

### Buff

描述:

包含完整生命周期的Buff组件. 支持不同Buff行为自由组合,同种Buff行为多套参数.

相关对象: BuffEntity IBuffAction BaseBuffActionAttribute BaseBuffAction BuffContainer BuffActionDispatcher

BuffEntity Buff实体类 记录了Buff实体的信息,  Buff生命周期逻辑

IBuffAction: 定义了Buff行为Run方法的接口

BaseBuffActionAttribute: 用于标记BuffAction类

BaseBuffAction: 继承IBuffAction接口并标记BaseBuffActionAttribute的类  用于编写具体的基础Buff行为逻辑

BuffContainer: Buff容器组件, 任何实体只要添加该容器组件就可以响应BuffEntity的生命周期逻辑  隔离了Buff与Buff影响的实体.

BuffActionDispatcher Buff行为分发器 用于BuffEntity获取对应的BuffAction并执行

使用方法：

编写BaseBuffAction

```csharp
    [BaseBuffAction(1)]
    public class TestAddBuffAction : IBuffAction
    {
        public void Run(BuffEntity buffEntity, int[] argsList)
        {
            
        }
    }
```


添加Buff

```csharp
   GetComponent<BuffContainerComponent>().TryAddBuff(1, sourceEntity);
```

移除Buff

```csharp
   GetComponent<BuffContainerComponent>().TryRemoveBuff(buffEntityId);
```


