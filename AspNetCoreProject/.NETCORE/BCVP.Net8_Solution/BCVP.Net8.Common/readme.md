#### 说明描述

### 获取配置信息的方式
## Appsettings单例类方式
下载两个包：Microsoft.Extensions.Configuration.Binder 和 Microsoft.Extensions.Configuration.Json
编写 Appsettings类

## IOptions方式
下载两个包：Microsoft.Extensions.DependencyModel 和 Microsoft.Extensions.Options.ConfigurationExtensions
编写 ConfigurableOptions类 和 IConfigurableOptions接口
所有配置的实体都要继承IConfigurableOptions接口，比如 RedisOptions




