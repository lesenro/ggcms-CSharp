import React, { Component } from 'react';

export default class GroupBox extends Component {
  constructor(props) {
    super(props);
  }
  render() {
    const { title } = this.props;
    return (
      <div className="group-box">
        <div className="title">{title}</div>
        {this.props.children}
      </div>
    );
  }
}
