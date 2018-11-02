import React, { PureComponent } from 'react';
import moment from 'moment';
import { Table, Alert, Badge, Divider, Tag } from 'antd';
import { OperateHelper } from '../Operate/OperateSetting';

const statusMap = ['default', 'processing', 'success', 'error'];

class StandardTable extends PureComponent {
  state = {
    selectedRowKeys: [],
    totalCallNo: 0,
  };
  componentWillMount() {}

  componentWillReceiveProps(nextProps) {
    // clean state
    if (nextProps.selectedRows.length === 0) {
      this.setState({
        selectedRowKeys: [],
        totalCallNo: 0,
      });
    }
  }

  handleRowSelectChange = (selectedRowKeys, selectedRows) => {
    const totalCallNo = selectedRows.reduce((sum, val) => {
      return sum + parseFloat(val.callNo, 10);
    }, 0);

    if (this.props.onSelectRow) {
      this.props.onSelectRow(selectedRows);
    }

    this.setState({ selectedRowKeys, totalCallNo });
  };

  handleTableChange = (pagination, filters, sorter) => {
    this.props.onChange(pagination, filters, sorter);
  };

  cleanSelectedKeys = () => {
    this.handleRowSelectChange([], []);
  };

  render() {
    const { selectedRowKeys, totalCallNo } = this.state;
    const { data: { pageData }, loading } = this.props;
    let pdata = pageData || {};
    const list = pdata.records;
    const pagination = {
      current: pdata.current,
      pageSize: pdata.size,
      total: pdata.total,
      // pages: pdata.pages,
    };
    const columns = [
      {
        title: '订单编号',
        dataIndex: 'orderNo',
      },
      {
        title: '姓名',
        dataIndex: 'sendName',
      },
      {
        title: '手机',
        dataIndex: 'sendTel',
      },
      {
        title: '日期',
        dataIndex: 'updateTime',
        render: val => <span>{moment(val).format('YYYY-MM-DD')}</span>,
      },
      {
        title: '总价',
        dataIndex: 'orderAmount',
      },
      {
        title: '人次',
        dataIndex: 'ticketNum',
      },
      {
        title: '操作',
        render: (text, record, index) =>
          OperateHelper.operateButtonShow(this.props.operateBtns, this.props.onButtonClick, record),
      },
    ];
    const paginationProps = {
      ...pagination,
    };

    const rowSelection = {
      selectedRowKeys,
      onChange: this.handleRowSelectChange,
      getCheckboxProps: record => ({
        disabled: record.disabled,
      }),
    };

    return (
      <div>
        <Table
          loading={loading}
          rowKey={record => record.orderNo}
          rowSelection={null}
          dataSource={list}
          columns={columns}
          pagination={paginationProps}
          onChange={this.handleTableChange}
        />
      </div>
    );
  }
}

export default StandardTable;
