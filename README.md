# SJET Framework
本项目是基于[ET6.0框架](https://github.com/egametang/ET) 的个人修改版框架

项目目标是提供更完善的客户端功能以及分享在ET框架下的一些组件编写思路

## 框架层

### 资源管理



按目录标记AssetBundle功能， 同个目录下的资源打进同个AB中。

编辑器下AB循环依赖分析功能。

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



### 本地化

## 业务层

### Buff







