# OpenIddict EF Core 迁移指南

## 📋 创建迁移

在项目根目录执行以下命令：

```bash
# 进入 WebApi 项目目录
cd backend/src/Takt.WebApi

# 添加 EF Core CLI 工具（如果没有）
dotnet tool install --global dotnet-ef

# 创建迁移
dotnet ef migrations add InitOpenIddict --context TaktOpenIddictContext --output-dir Data/Migrations

# 应用迁移（创建数据库）
dotnet ef database update --context TaktOpenIddictContext
```

## 🔧 手动创建迁移文件

如果不想使用命令行，可以手动创建迁移文件。
