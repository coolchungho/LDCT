# 給助理／代理的指引

## 規格優先

- 新功能或行為變更：先於 `openspec/changes/<change-name>/` 撰寫 **Delta**（ADDED／MODIFIED／REMOVED），勿將 `openspec/specs/` 當未實作草稿。
- 內文以繁體中文為主；`### Requirement:`、SHALL／MUST、GIVEN／WHEN／THEN、Delta 區段標題維持英文慣例以利工具相容。

## 實作時

- 變更完成並部署後，以 `openspec archive <change> --yes` 合併至 `openspec/specs/`，再移除該 change 資料夾。
- 若專案根目錄有 [計畫書.md](../計畫書.md)，實作功能時應同步更新計畫書對應章節。

## 參考

- OpenSpec 技能：使用者 Cursor skills 內 `openspec-prd`
- 官方概念：<https://thedocs.io/openspec/>
