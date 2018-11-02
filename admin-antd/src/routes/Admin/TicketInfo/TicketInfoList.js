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
        title: '编号',
        dataIndex: 'productCode',
      },
      {
        title: '公园',
        dataIndex: 'parkName',
      },
      {
        title: '票种',
        dataIndex: 'productName',
      },
      {
        title: '日期',
        dataIndex: 'date',
        render: val => <span>{moment(val).format('YYYY-MM-DD')}</span>,
      },
      {
        title: '价格',
        dataIndex: 'productPrice',
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
