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
import CustomerList from './CustomerList';
import GridForm from '../../../components/GridForm/GridForm';

const confirm = Modal.confirm;
const FormItem = Form.Item;
const { Option } = Select;
const getValue = obj =>
  Object.keys(obj)
    .map(key => obj[key])
    .join(',');

@connect(state => state.user)
export default class Customer extends PureComponent {
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
    setTimeout(() => {
      this.getDataList();
    }, 100);
  }
  //获取分页数据
  getDataList = pageNum => {
    pageNum = pageNum || this.state.pageNum;
    this.setState({ pageNum: pageNum });
    const { dispatch } = this.props;
    var temp = Object.assign({}, this.state.formValues);
    if (temp.createTime) {
      temp.createTime = temp.createTime.format('YYYY-MM-DD');
    }
    dispatch({
      type: 'user/getCustomer',
      payload: {
        currentPage: pageNum,
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

  //搜索
  handleSearch = e => {
    // e.preventDefault();
    let values = this.searchFrm.ConvertToObject(e);
    if (values.name || values.tel) {
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
    const { records } = this.props;
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
              span: 12,
              className: 'padding',
            },
            Caption: '',
          },
          {
            GroupType: 'col',
            Name: 'form1_2',
            Props: {
              span: 12,
              className: 'padding',
            },
            Caption: '',
          },
        ],
      },
      Items: [
        {
          InputType: 'Text',
          Name: 'name',
          GroupName: 'form1_1',
          Icon: 'user',
          ItemProps: {
            label: '姓名:',
          },
          Props: {
            placeholder: '请输入姓名',
            // "disabled": true,
          },
          Rules: [
            {
              max: 10,
              message: '姓名应在10个字符以内',
            },
          ],
        },
        {
          InputType: 'Text',
          Name: 'tel',
          ItemProps: {
            label: '手机号:',
          },
          Icon: 'mobile',
          GroupName: 'form1_2',
          Props: {
            placeholder: '请输入手机号',
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
          className: 'text-right',
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

  render() {
    const { loading: ruleLoading, data } = this.props;
    const { selectedRows, modalVisible, editInfo, roles } = this.state;
    return (
      <PageHeaderLayout>
        <Card bordered={false}>
          <div className={styles.tableList}>
            <div className={styles.tableListForm}>{this.renderToolsbar()}</div>
            {this.state.expandForm && (
              <div className={styles.tableListForm}>{this.renderAdvancedForm()}</div>
            )}

            <CustomerList
              selectedRows={selectedRows}
              loading={ruleLoading}
              data={data}
              onChange={this.handleTableChange}
              // operateBtns={records.operateAuthorities}
            />
          </div>
        </Card>
      </PageHeaderLayout>
    );
  }
}
