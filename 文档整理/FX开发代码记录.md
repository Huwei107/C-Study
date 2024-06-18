1、获取随机id：HelperGUID.GetGuid();
   获取流水号SQL.GetCode();
   获取系统当前用户登录名CurrentUser.RealName.ToString();
   通过字典获取产品业务数量默认单位 ExtendProducts.GetBusinessAmountDefaultUnit();
   dto等实体中日期用string类型;
**********************************************************************************************************************************************************
2、导出方法：1. 简单导出：c1TrueDBGrid1.C1GridToExcel(this.Text);
            2. 合并列导出：c1TrueDBGrid1.C1GridToExcelWithMerge(this.Text,"起始列名");
**********************************************************************************************************************************************************
3、工品一号产品的品名（BPTNAME）就是标准，中文描述（BPTCNDescription）就是品名
   真实库存：
   虚拟库存：
   可用库存：产品的库存 - 销售订单中未结案的数量之和（数量 = 订单数 - 取货数 - 送货数）（已审核，订单退单都算）
   其他均归为在途库存
   SKU,SPU
   出入库时，操作Whs_ProductLocationStock产品库位库存表，Whs_ProductLoactionBalances产品库位平衡表，Whs_ProductStock产品库存表，Whs_ProductBalances产品出入库平衡表表
   销售订单审核后，那部分库存会被冻结
**********************************************************************************************************************************************************
4、打印、预览和打印配置功能，页面比如采购待收货明细表(FrmWhs_WaitBuycomfirmList)、销售待收货(FrmWhs_WaitSelReturnList)，需要配置print.ini文件，增加一个模板
   打印功能：（HelperPrint工具类）
   var reportinfo = ExtendReportMangerInfo.getModelSetInfo("采购模板类", printtarget, string.Empty, customerid.ToString(), "");（获取打印模板）;
   HelperPrint.PrintOrPreviewWorkOrderDev(printtarget, dt, false, false, false, true, false, true, reportinfo);（打印方法）;
**********************************************************************************************************************************************************
5、SQL语句中数量的计算尽量用ISNULL(Convert(decimal(18,3), value), 0)转换一下再计算;
   SQL语句中WHERE条件后的数量比较要确保数量字段不为null，用ISNULL(value, 0);
   SQL语句中判断数量是否为空或为0，用 ISNULL(value, 0) = 0; ISNULL起作用的前提是这一行数据存在;
   数据的传递尽量不要用datatable和dataset，尽量用实体集

   Datatable连续删除行操作时，其索引会改变，会导致有些行永远遍历不到，删除后需要进行判断操作;

   DataSet增加一个Datatable：dt.TableName="", ds.Tables.Add(dt.Copy());

   Datatable 新增空行 DataRow dr = dt.NewRow(); dr["Name"] = "";  dt.Rows.InsertAt(dr, 0);


**********************************************************************************************************************************************************
6、对C1列表操作：
   获取C1列表勾选行 if (c1TrueDBGrid1[i, "选择"].ToString().ToBoolean());
   获取当前选中行 int index = c1TrueDBGrid1.Row;
   获取C1列表所有行数 c1TrueDBGrid1.Splits[0].Rows.Count;
   设置C1列字段几位小数 c1TrueDBGrid1.SetAccurate("列1|列2", 4)，4位小数点;
   给C1列表赋值 InitGridView<ProductLocationDto>(c1TrueDBGrid1, dt);
   给C1列表设置精度 c1TrueDBGrid1.SetAccurate(nameof(OrdersInfoDto.SODTotalWeight), ExtendProducts.GetProductBusinessWeightForDataGridControl());
   给C1列表设置模糊查询 c1TrueDBGrid1.FilterBar = true;

   设置C1列可编辑 c1TrueDBGrid1.SetAllLock("列名");
   设置C1所有列不可编辑 c1TrueDBGrid1.SetAllLock();
   设置C1列不可编辑 c1TrueDBGrid2.SetLock("列名1|列名2");
   设置C1列自适应 c1TrueDBGrid1.AutoSize();

   c1TrueDBGrid1_FetchCellStyle事件：对单个单元格改变颜色等; c1TrueDBGrid1.FetchCellStyle = true
   c1TrueDBGrid1.Splits[0].DisplayColumns[nameof(OrdersListDto.SODBuyDeliveryDate)].FetchStyle = true;
   c1TrueDBGrid1_FetchRowStyle事件：对整行单元格改变颜色等; c1TrueDBGrid1.FetchRowStyle = true

   c1TrueDBGrid1.update()：实时更新列表填写的数据;

   c1TrueDBGrid1.Columns[nameof(OrdersListDto.SOMCreateTime)].NumberFormat = "yyyy-MM-dd HH:mm:ss"：设置时间格式
   InitGridView<ProductMaterialRelationDto>(c1TrueDBGrid1, new List<ProductMaterialRelationDto>())：初始化表头
   InitGridView<SellOutboundFrequencyListDto>(c1TrueDBGrid1, dt, addOrderNumber:false)：去除序号列

   if (c1TrueDBGrid1.Splits[0].Rows.Count <= 0) return; 判断C1列表是否为空

   c1TrueDBGrid1.Splits[0].DisplayColumns[nameof(ProductMaterialRelationDto.MaterialBarCode)].DataColumn.Caption = "模具条码";---------手动修改列头名称

   c1TrueDBGrid1属性：
    1. AllowAddNew：自动增加下一行

   ButttonVisible.ControlsReadOnlyCanCopy(panelMain.Controls, false, false, GetCanCopyControls()); 根据状态显示控件是否可编辑
   this.SetEnableButton(false, ButtonEnumList.直接出库);手动显示按钮
   DisableButtonForAuditStatus(MainEntity.WPBStatus); 根据状态是否隐藏按钮
   ButttonVisible.ControlsReadOnly(panelMain.Controls, false, false);
**********************************************************************************************************************************************************
7、分页功能：
   代表页面FrmBas_UserProductRelationList(用户产品绑定页面)
   1. 在页面的Load页面中添加  cbxPageSize.DataSource = new List<int>() { 100, 1000, 10000 };（每页的显示数量）
                            cbxPageSize.SelectedIndex = 0;（进入页面从首页开始，页码的value从0开始，显示从1开始）
                            cbxPageSize.SelectedIndexChanged += cbxPageSize_SelectedIndexChanged;（手动添加事件）
   2. 在主体查询中添加代码
   3. 添加事件 private void cbxPageSize_SelectedIndexChanged，分页每页显示数量
   4. 添加事件 private void c1DbNavigator1_PositionChanged，页码
**********************************************************************************************************************************************************
8、单元格下拉框代码：
**********************************************************************************************************************************************************
9、combobox和treeview

**********************************************************************************************************************************************************
10、正则验证
    Regex objReg = new Regex(@"^[0-9]+(.[0-9]{1,3})?$");---------数字（含小数）,最多三位小数
**********************************************************************************************************************************************************
11、FX单据状态设置页面是否可编辑等（页面：FrmSel_OrdersInfo）
  1. 文本框
   ButttonVisible.ControlsReadOnlyCanCopy(panel3.Controls, true, false, GetCanCopyControls());
   ButttonVisible.ControlsReadOnly(pnlPrepay.Controls, true, false, true);
  2. 按钮
   SetEnableButton(false, ButtonEnumList.审核);
**********************************************************************************************************************************************************
12、删除临时表
IF OBJECT_ID('tempdb..#temp') IS NOT NULL
    DROP TABLE #temp
**********************************************************************************************************************************************************
13、转义字符如：\',\",\\，分别表示单引号，双引号，反斜杠
**********************************************************************************************************************************************************
14、贸易系统：
  1. 参考的价格都是含税的
  2. 加权的价格都是未税的
  3. 销售总额
**********************************************************************************************************************************************************
15、代码记录
  1. FormCollection formCollection = Application.OpenForms;//获取存在的窗体集合
  2. System.Reflection.Assembly ab = System.Reflection.Assembly.GetExecutingAssembly();
     Form frm = (Form)ab.CreateInstance(strs[0]); //根据反射创建页面
**********************************************************************************************************************************************************
16、计算
  1. Math.Round：四舍六入五取整
  2. Math.Ceiling：向上取整，只要有小数都加1
  3. Math.Floor：向下取整，总是舍去小数
**********************************************************************************************************************************************************
/// <summary>
/// 获取属性的值
/// </summary>
/// <typeparam name="T">实体类</typeparam>
/// <param name="t">实体</param>
/// <param name="property">属性名称</param>
/// <returns></returns>
 public static Dictionary<object, object> GetPropertieValue<T>(T t,string property)
 {
     var ret = new Dictionary<object, object>();
     if (t == null) { return null; }
     PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
     if (properties.Length <= 0) { return null; }

     foreach (PropertyInfo item in properties)
     {
         string name = item.Name;
         object value = item.GetValue(t, null);
         if (item.Name == property)
         {
             ret.Add(name, value);
         }
     }
     return ret;
 }

**********************************************************************************************************************************************************
**********************************************************************************************************************************************************
**********************************************************************************************************************************************************
**********************************************************************************************************************************************************
