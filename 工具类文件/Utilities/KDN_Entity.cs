using System;
using System.Collections.Generic;
using System.Text;

namespace FX.MainForms
{
    /// <summary>
    /// 快递鸟接口
    /// </summary>
    public class KDN_Entity
    {
        /// <summary>
        /// 快递公司编码
        /// </summary>
        public string ShipperCode { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 邮费支付方式:1-现付，2-到付，3-月结，4-第三方支付(仅SF支持)
        /// </summary>
        public int PayType { get; set; }

        /// <summary>
        /// 快递类型：1-标准快件 ,详细快递类型参考《快递公司快递业务类型.xlsx》
        /// </summary>
        public string ExpType { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 客户密码
        /// </summary>
        public string CustomerPwd { get; set; }

        /// <summary>
        /// SendSite
        /// </summary>
        public string SendSite { get; set; }

        /// <summary>
        /// SendStaff
        /// </summary>
        public string SendStaff { get; set; }

        /// <summary>
        /// MonthCode
        /// </summary>
        public string MonthCode { get; set; }

        /// <summary>
        /// Cost
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        /// OtherCost
        /// </summary>
        public double OtherCost { get; set; }

        /// <summary>
        /// sender
        /// </summary>
        public Sender Sender { get; set; }

        /// <summary>
        /// sender
        /// </summary>
        public Receiver Receiver { get; set; }

        /// <summary>
        /// Weight
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Quantity
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Volume
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// IsReturnPrintTemplate
        /// </summary>
        public string IsReturnPrintTemplate { get; set; }

        public List<Commodity> Commodity { get; set; }
    }

    /// <summary>
    /// 发件信息
    /// </summary>
    public class Sender
    {
        /// <summary>
        /// 发件人公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 发件人
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 发件省 (如广东省，不要缺少“省”； 如是直辖市，请直接传北京、上海等； 如是自治区，请直接传广西壮族自治区等)
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 发件市(如深圳市，不要缺少“市； 如是市辖区，请直接传北京市、上海市等”)
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 发件区/县(如福田区，不要缺少“区”或“县”)
        /// </summary>
        public string ExpAreaName { get; set; }
        /// <summary>
        /// 发件人详细地址
        /// </summary>
        public string Address { get; set; }
    }

    /// <summary>
    /// 发件信息
    /// </summary>
    public class Receiver
    {
        /// <summary>
        /// 收件人公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 收件省 (如广东省，不要缺少“省”；如是直辖市，请直接传北京、上海等； 如是自治区，请直接传广西壮族自治区等)
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 收件市(如深圳市，不要缺少“市”； 如果是市辖区，请直接传北京市、上海市等)
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 收件区/县(如福田区，不要缺少“区”或“县”)
        /// </summary>
        public string ExpAreaName { get; set; }
        /// <summary>
        /// 收件人详细地址
        /// </summary>
        public string Address { get; set; }
    }

    /// <summary>
    /// 产品
    /// </summary>
    public class Commodity
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public string Goodsquantity { get; set; }
        /// <summary>
        /// 商品重量kg
        /// </summary>
        public string GoodsWeight { get; set; }
    }

    public class KDN_Back
    {
        public string PrintTemplate { get; set; }

        public string EBusinessID { get; set; }

        public string UniquerRequestNumber { get; set; }

        public string ResultCode { get; set; }

        public string Reason { get; set; }

        public bool Success { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public Order order { get; set; }

        public string SubOrders { get; set; }
    }

    public class Order
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 快递公司编码
        /// </summary>
        public string ShipperCode { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string LogisticCode { get; set; }

        /// <summary>
        /// 大头笔
        /// </summary>
        public string MarkDestination { get; set; }

        /// <summary>
        /// 签回单单号
        /// </summary>
        public string SignWaybillCode { get; set; }

        /// <summary>
        /// 始发地区域编码
        /// </summary>
        public string OriginCode { get; set; }

        /// <summary>
        /// 始发地/始发网点
        /// </summary>
        public string OriginName { get; set; }

        /// <summary>
        /// 目的地区域编码
        /// </summary>
        public string DestinatioCode { get; set; }

        /// <summary>
        /// 目的地/到达网点
        /// </summary>
        public string DestinatioName { get; set; }

        /// <summary>
        /// 分拣编码
        /// </summary>
        public string SortingCode { get; set; }

        /// <summary>
        /// 集包编码
        /// </summary>
        public string PackageCode { get; set; }

        /// <summary>
        /// 集包地
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        /// 目的地分拨
        /// </summary>
        public string DestinationAllocationCentre { get; set; }

        /// <summary>
        /// 设计模板用(仅ShipperCode为SF时返回)
        /// </summary>
        public string ShipperInfo { get; set; }

        /// <summary>
        /// 成功与否(true/false)
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 返回编码
        /// </summary>
        public string ResultCode { get; set; }

        /// <summary>
        /// 失败原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 唯一标示
        /// </summary>
        public string UniquerRequestNumber { get; set; }
    }
}
