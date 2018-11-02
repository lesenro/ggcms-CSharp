import React from 'react';
import { Row, Col, Card, Tooltip } from 'antd';

export default class HomePage extends React.Component {
  state = {
    operates: [],
    confirmDirty: false,
  };

  componentDidMount() {
    this.setState({
      ...this.props.data,
    });
  }
  render() {
    const appcfg = window.AppConfigs;
    return (
      <Card title="管理首页" bordered={false}>
        <div className="banner-title-wrapper">
          <div className="title-line-wrapper">
            <div className="title-line" />
          </div>
          <h1>
            {appcfg.Company_short_name}-{appcfg.System_name}
          </h1>
          <p>
            <span>欢迎使用{appcfg.System_name}</span>
          </p>
        </div>
      </Card>
    );
  }
}
