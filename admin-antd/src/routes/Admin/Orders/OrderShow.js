import React, { PureComponent } from 'react';
import { Form, Icon, Input, Button, Checkbox, Modal, Select, Table, Tag, Row, Col } from 'antd';
import moment from 'moment';

const Option = Select.Option;
const CheckboxGroup = Checkbox.Group;
const FormItem = Form.Item;
const { TextArea } = Input;

@Form.create()
export default class OrderShow extends PureComponent {
  state = {
    checkedMenus: [],
    menu: [],
  };

  handleSubmit = e => {
    e.preventDefault();
    this.props.onSubmit({ type: 'success', data: {} });
  };
  //   componentWillReceiveProps(nextProps) {     //this.props.form.resetFields();
  //     this.setState({...nextProps.data},()=>{     });   }
  componentDidMount() {
    this.setState({
      ...this.props.data,
    });
  }
  render() {
    const { getFieldDecorator } = this.props.form;
    const { roles, data: datainfo } = this.props;
    const formItemLayout = {
      labelCol: {
        xs: { span: 24 },
        sm: { span: 6 },
      },
      wrapperCol: {
        xs: { span: 24 },
        sm: { span: 18 },
      },
    };
    const columns = [
      {
        title: '串码（二维码）',
        dataIndex: 'ecode',
      },
      {
        title: '名称',
        dataIndex: 'productName',
      },
      {
        title: '日期',
        dataIndex: 'productSDate',
      },
      {
        title: '单价',
        dataIndex: 'productPrice',
      },
      {
        title: '人次',
        dataIndex: 'productCount',
      },
      {
        title: '总价',
        render: (text, record, index) => <div>{record.productPrice * record.productCount}</div>,
      },
      {
        title: '状态',
        render: (text, record, index) => (
          <div>
            {/* F：已释放订单，B：作废，R：取消，N:未付款， H：已取纸质票，T:线上已检，O：线下已检 M 已退款,E退款审核中，AA部分退款，AB退款失败 */}
            {record.itemType == 'AB' && <Tag color="red">退款后入园</Tag>}
            {record.itemType == 'B' && <Tag color="red">作废</Tag>}
            {record.itemType == 'M' && <Tag color="red">已退款</Tag>}
            {record.itemType == 'R' && <Tag color="red">取消</Tag>}
            {record.itemType == 'F' && <Tag color="gold">已释放</Tag>}
            {record.itemType == 'N' && <Tag color="gold">未付款</Tag>}
            {record.itemType == 'E' && <Tag color="gold">退款审核中</Tag>}
            {record.itemType == 'AA' && <Tag color="blue">部分退款</Tag>}
            {record.itemType == 'T' && <Tag color="blue">线上已检</Tag>}
            {record.itemType == 'O' && <Tag color="green">线下已检</Tag>}
            {record.itemType == 'H' && <Tag color="green">已取纸质票</Tag>}
            {record.itemType == 'S' && <Tag color="green">已付款</Tag>}
            {record.itemType == 'G' && <Tag color="green">已改签</Tag>}
          </div>
        ),
      },
    ];
    return (
      <Modal
        width="917px"
        title="订单详情"
        visible={this.props.modalVisible}
        onOk={this.handleSubmit}
        onCancel={() => {
          this.props.onSubmit({ type: 'cancel' });
        }}
      >
        <Form>
          <Row>
            <Col span="12">
              <FormItem {...formItemLayout} label="订单编号">
                <div className="ant-input">{datainfo.orderNo}</div>
              </FormItem>
            </Col>
            <Col span="12">
              <FormItem {...formItemLayout} label="用户编号">
                <div className="ant-input">{datainfo.openid}</div>
              </FormItem>
            </Col>
          </Row>
          <Row>
            <Col span="12">
              <FormItem {...formItemLayout} label="姓名">
                <div className="ant-input">{datainfo.sendName}</div>
              </FormItem>
            </Col>
            <Col span="12">
              <FormItem {...formItemLayout} label="手机">
                <div className="ant-input">{datainfo.sendTel}</div>
              </FormItem>
            </Col>
          </Row>
          <Row>
            <Col span="12">
              <FormItem {...formItemLayout} label="人次">
                <div className="ant-input">{datainfo.ticketNum}</div>
              </FormItem>
            </Col>
            <Col span="12">
              <FormItem {...formItemLayout} label="价格">
                <div className="ant-input">{datainfo.orderAmount}</div>
              </FormItem>
            </Col>
          </Row>
          <Row>
            <Col span="12">
              <FormItem {...formItemLayout} label="日期">
                <div className="ant-input">{moment(datainfo.updateTime).format('YYYY-MM-DD')}</div>
              </FormItem>
            </Col>
            <Col span="12">
              <FormItem {...formItemLayout} label="状态">
                {datainfo.payStatus == 0 && <Tag color="magenta">未付款</Tag>}
                {datainfo.payStatus == 1 && <Tag color="green">已付款</Tag>}
              </FormItem>
            </Col>
          </Row>
        </Form>
        <Table
          pagination={false}
          rowKey={record => record.itemID}
          dataSource={datainfo.detail}
          columns={columns}
        />
      </Modal>
    );
  }
}
