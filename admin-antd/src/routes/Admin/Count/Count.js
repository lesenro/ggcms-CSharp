import React, { PureComponent } from 'react';
import { stringify } from 'qs';
import { connect } from 'dva';
import {
  Row,
  Col,
  Card,
  Form,
  Input,
  Select,
  Icon,
  Button,
  Dropdown,
  Menu,
  InputNumber,
  DatePicker,
  Modal,
  message,
} from 'antd';
import PageHeaderLayout from '../../../layouts/PageHeaderLayout';
import styles from '../admin.less';
import CountList from './CountList';
import GridForm from '../../../components/GridForm/GridForm';
import moment from 'moment';
import { Chart, Geom, Axis, Tooltip, Legend, Coord } from 'bizcharts';
import { DataSet } from '@antv/data-set';

const confirm = Modal.confirm;
const FormItem = Form.Item;
const { Option } = Select;
const getValue = obj =>
  Object.keys(obj)
    .map(key => obj[key])
    .join(',');

@connect(state => state.count)
@Form.create()
export default class CountComponent extends PureComponent {
  state = {
    modalVisible: false,
    expandForm: false,
    selectedRows: [],
    formValues: {},
    editInfo: {},
    roles: [],
    pageNum: 1,
  };

  componentDidMount() {
    const { dispatch } = this.props;
    let now = moment();
    let vals = {
      endTime: now.clone(),
      startTime: now.subtract(1, 'months'),
    };
    this.searchFrm.SetValues(vals);
    this.setState(
      {
        formValues: vals,
      },
      () => {
        setTimeout(() => {
          this.getDataList();
        }, 100);
      }
    );
  }
  //获取分页数据
  getDataList = pageNum => {
    pageNum = pageNum || this.state.pageNum;
    this.setState({ pageNum: pageNum });
    const { dispatch } = this.props;
    let values = this.state.formValues;
    var temp = {
      startTime: values.startTime.format('YYYY-MM-DD HH:mm:ss'),
      endTime: values.endTime.format('YYYY-MM-DD HH:mm:ss'),
    };

    dispatch({
      type: 'count/getDetailTotal',
      payload: {
        // currentPage: pageNum,
        ...temp,
      },
    });
  };
  //分页修改
  handleTableChange = (pagination, filters, sorter) => {
    this.getDataList(pagination.current);
  };
  //重置筛选
  handleFormReset = () => {
    const { form, dispatch } = this.props;
    form.resetFields();
    this.setState(
      {
        formValues: {},
      },
      () => {
        this.getDataList(1);
      }
    );
  };
  //切换显示筛选表单
  toggleForm = () => {
    this.setState({
      expandForm: !this.state.expandForm,
    });
  };
  //操作
  handleActionClick = e => {
    const { dispatch } = this.props;
    const { selectedRows } = this.state;
    switch (e.code) {
      case 'add':
        this.setState({
          editInfo: {
            userName: '',
            loginName: '',
            password: '',
            state: 0,
            mobile: '',
            email: '',
            role: '',
            id: '',
          },
          modalVisible: true,
        });
        break;
      default:
        break;
    }
  };
  //选择记录
  handleSelectRows = rows => {
    this.setState({
      selectedRows: rows,
    });
  };
  //搜索
  handleSearch = e => {
    // e.preventDefault();
    let values = this.searchFrm.ConvertToObject(e);
    if (values.startTime && values.endTime) {
      this.setState(
        {
          formValues: values,
        },
        () => {
          this.getDataList(1);
        }
      );
    }
  };
  //工具条渲染
  renderToolsbar() {
    const { getFieldDecorator } = this.props.form;

    return (
      <Row gutter={{ md: 8, lg: 24, xl: 48 }}>
        <Col md={16} sm={24}>
          <div className={styles.tableListOperator} />
        </Col>
        <Col md={8} sm={24} className={'text-right'}>
          <span className={styles.submitButtons}>
            {this.state.formValues.hasOwnProperty('updatedAt') && (
              <Button style={{ marginLeft: 8 }} onClick={this.handleFormReset}>
                重置
              </Button>
            )}
            {this.state.expandForm ? (
              <a style={{ marginLeft: 8 }} onClick={this.toggleForm}>
                收起 <Icon type="up" />
              </a>
            ) : (
              <a style={{ marginLeft: 8 }} onClick={this.toggleForm}>
                筛选 <Icon type="down" />
              </a>
            )}
          </span>
        </Col>
      </Row>
    );
  }
  //筛选表单
  renderAdvancedForm() {
    const { getFieldDecorator } = this.props.form;
    const { loading } = this.props;
    const searchModel = {
      Groups: {
        GroupType: 'row',
        Name: 'form1',
        Caption: '搜索框',
        Children: [
          {
            GroupType: 'col',
            Name: 'form1_1',
            Props: {
              span: 10,
              className: 'padding',
            },
            Caption: '',
          },
          {
            GroupType: 'col',
            Name: 'form1_2',
            Props: {
              span: 10,
              className: 'padding',
            },
            Caption: '',
          },
        ],
      },
      Items: [
        {
          InputType: 'DateTimePicker',
          Name: 'startTime',
          GroupName: 'form1_1',
          Icon: 'calendar',
          ItemProps: {
            label: '开始时间:',
          },
          Props: {
            placeholder: '请输入开始时间',
            showTime: true,
            format: 'YYYY-MM-DD HH:mm:ss',
          },
        },
        {
          InputType: 'DateTimePicker',
          Name: 'endTime',
          ItemProps: {
            label: '结束时间:',
          },
          Icon: 'calendar',
          GroupName: 'form1_2',
          Props: {
            placeholder: '请输入结束时间',
            showTime: true,
            format: 'YYYY-MM-DD HH:mm:ss',
          },
        },
      ],
      Global: {
        GroupsProps: {},
        ItemProps: {
          labelCol: { span: 6 },
          wrapperCol: { span: 14 },
        },
        ElementProps: {},
        SubmitItemProps: {
          isTop: true,
          style: {
            position: 'absolute',
            bottom: '10px',
            right: '15px',
            zIndex: 15,
          },
        },
        SubmitButtonProps: {
          className: styles.submit,
          type: 'primary',
          text: (
            <span>
              <Icon type="search" /> 搜索
            </span>
          ),
        },
      },
    };
    return (
      <GridForm
        onError={ev => {
          console.log(ev);
        }}
        loading={loading}
        tmpl={searchModel}
        submit={this.handleSearch}
        ref={frm => {
          this.searchFrm = frm;
        }}
      />
    );
  }
  renderChart(list) {
    let data = [
      {
        name: '订单',
      },
      {
        name: '人次',
      },
    ];
    (list || []).forEach(x => {
      data[0][x.productName] = x.countOrder;
      data[1][x.productName] = x.sumTicket;
    });

    const ds = new DataSet();
    const dv = ds.createView().source(data);
    dv.transform({
      type: 'fold',
      fields: (list || []).map(x => x.productName), // 展开字段集
      key: '票种', // key字段
      value: '销量', // value字段
    });
    return (
      <Chart height={400} data={dv} forceFit>
        <Axis name="票种" />
        <Axis name="销量" />
        <Legend />
        <Tooltip crosshairs={{ type: 'y' }} />
        <Geom
          type="interval"
          position="票种*销量"
          color={'name'}
          adjust={[{ type: 'dodge', marginRatio: 1 / 32 }]}
        />
      </Chart>
    );
  }
  render() {
    const { loading: ruleLoading, data } = this.props;
    const { selectedRows, modalVisible, editInfo, roles } = this.state;
    let list = data.pageData || [];
    return (
      <PageHeaderLayout>
        <Card bordered={false}>
          <div className={styles.tableList}>
            <div className={styles.tableListForm}>
              <div className={styles.tableListForm}>{this.renderAdvancedForm()}</div>
            </div>
            <Row>
              <Col span={list.length > 5 ? 24 : 12}>{this.renderChart(list)}</Col>
              <Col span={list.length > 5 ? 24 : 12}>
                <CountList
                  selectedRows={selectedRows}
                  loading={ruleLoading}
                  data={data}
                  onSelectRow={this.handleSelectRows}
                  onButtonClick={this.handleActionClick}
                  onChange={this.handleTableChange}
                  // operateBtns={records.operateAuthorities}
                />
              </Col>
            </Row>
          </div>
        </Card>
      </PageHeaderLayout>
    );
  }
}
