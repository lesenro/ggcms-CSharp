import React, { Component } from 'react';
import FormGroup from './FormGroup';
import { Form, Button } from 'antd';
import _ from 'lodash';
const FormItem = Form.Item;
export default class GridForm extends Component {
  constructor(props) {
    super(props);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.SetValues = this.SetValues.bind(this);
  }
  SetValues(obj) {
    for (const key in obj) {
      this.groupFrm.SetValue(key, obj[key]);
    }
  }
  SetOptions(obj) {
    for (const key in obj) {
      this.groupFrm.SetOptions(key, obj[key]);
    }
  }
  ConvertToObject(arrList, nameStr = 'name', valueStr = 'value') {
    let obj = {};
    arrList.forEach(item => {
      obj[item[nameStr]] = item[valueStr];
    });
    return obj;
  }
  handleSubmit(ev) {
    ev.preventDefault();
    //全部校验
    this.groupFrm.Validate();
    var errs = this.groupFrm.GetErrors();
    errs = _.flattenDeep(errs).filter(x => x);
    if (errs.length > 0) {
      if (this.props.onError) {
        this.props.onError(errs);
      }
      return;
    }
    //获取表单值
    var values = this.groupFrm.GetValues();
    values = _.flattenDeep(values);
    if (this.props.submit) {
      this.props.submit(values);
    }
  }
  componentWillReceiveProps() {}
  render() {
    const { tmpl } = this.props;
    const submitItem = tmpl.Global.SubmitItemProps || {};
    const submitBtn = tmpl.Global.SubmitButtonProps || {};
    return (
      <Form onSubmit={this.handleSubmit} className="grid-form">
        {submitItem.isTop && (
          <FormItem {...submitItem}>
            <Button loading={this.props.loading} {...submitBtn} htmlType="submit">
              {submitBtn.text || '提交'}
            </Button>
          </FormItem>
        )}
        <FormGroup
          onChange={ev => {
            if (this.props.onItemChange) {
              this.props.onItemChange(ev);
            }
          }}
          group={tmpl.Groups}
          items={tmpl.Items}
          global={tmpl.Global}
          ref={frm => {
            this.groupFrm = frm;
          }}
        />
        {!submitItem.isTop && (
          <FormItem {...submitItem}>
            <Button loading={this.props.loading} {...submitBtn} htmlType="submit">
              {submitBtn.text || '提交'}
            </Button>
          </FormItem>
        )}
      </Form>
    );
  }
}
