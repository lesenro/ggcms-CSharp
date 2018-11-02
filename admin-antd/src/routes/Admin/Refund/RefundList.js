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
        dataIndex: 'linkName',
      },
      {
        title: '手机',
        dataIndex: 'linkPhone',
      },
      {
        title: '退款日期',
        dataIndex: 'refundTime',
        render: val => <span>{moment(val).format('YYYY-MM-DD HH:mm:ss')}</span>,
      },
      {
        title: '退票数量',
        dataIndex: 'refundCount',
      },
      {
        title: '退款单价',
        dataIndex: 'refundPrice',
      },
      {
        title: '是否审核',
        render: (text, record, index) => (
          <div>
            {record.isAudit == 0 && <Tag color="green">不需要审核</Tag>}
            {record.isAudit == 1 && <Tag color="red">需要审核</Tag>}
          </div>
        ),
      },
      {
        title: '退票状态',
        render: (text, record, index) => (
          <div>
            {record.isRefund == 0 && <Tag color="gold">未退</Tag>}
            {record.isRefund == 1 && <Tag color="blue">已退</Tag>}
          </div>
        ),
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
          rowKey={record => record.id}
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
