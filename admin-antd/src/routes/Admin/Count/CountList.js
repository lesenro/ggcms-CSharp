import React, { PureComponent } from 'react';
import moment from 'moment';
import { Table, Alert, Badge, Divider, Tag } from 'antd';

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
    let pdata = pageData || [];
    const list = pdata;

    const columns = [
      {
        title: '票种',
        dataIndex: 'productName',
      },
      {
        title: '订单数',
        dataIndex: 'countOrder',
      },
      {
        title: '人次',
        dataIndex: 'sumTicket',
      },
    ];

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
          rowKey={record => record.productName}
          rowSelection={null}
          dataSource={list}
          columns={columns}
          pagination={false}
          onChange={this.handleTableChange}
        />
      </div>
    );
  }
}

export default StandardTable;
