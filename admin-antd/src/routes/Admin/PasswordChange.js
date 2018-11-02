import React, { PureComponent } from 'react';
import { Form, Icon, Input, Button, Checkbox, Modal, Select, Tree } from 'antd';

const Option = Select.Option;
const TreeNode = Tree.TreeNode;
const CheckboxGroup = Checkbox.Group;
const FormItem = Form.Item;

@Form.create()
export default class PasswordChange extends PureComponent {
  state = {
    operates: [],
    confirmDirty: false,
  };
  handleConfirmBlur = e => {
    const value = e.target.value;
    this.setState({ confirmDirty: this.state.confirmDirty || !!value });
  };
  handleSubmit = e => {
    e.preventDefault();
    this.props.form.validateFields((err, values) => {
      if (!err) {
        var senddata = {
          ...values,
        };
        this.props.onSubmit({ type: 'success', data: senddata });
      }
    });
  };
  checkPassword = (rule, value, callback) => {
    const form = this.props.form;
    if (value && value !== form.getFieldValue('password')) {
      callback('两次密码必须相同!');
    } else {
      callback();
    }
  };
  checkConfirm = (rule, value, callback) => {
    const form = this.props.form;
    if (value && this.state.confirmDirty) {
      form.validateFields(['repwd'], { force: true });
    }
    callback();
  };
  componentDidMount() {
    this.setState({
      ...this.props.data,
    });
  }
  render() {
    const { getFieldDecorator } = this.props.form;

    return (
      <Modal
        title="密码修改"
        visible={this.props.modalVisible}
        onOk={this.handleSubmit}
        onCancel={() => {
          this.props.onSubmit({ type: 'cancel' });
        }}
        afterClose={() => {
          this.props.form.resetFields();
        }}
      >
        <Form className="edit-form">
          <FormItem
            labelCol={{
              span: 5,
            }}
            wrapperCol={{
              span: 15,
            }}
            label="旧密码"
          >
            {getFieldDecorator('oldpassword', {
              rules: [
                {
                  required: true,
                  message: '必须输入旧密码!',
                },
              ],
            })(<Input type="password" placeholder="请输入旧密码" />)}
          </FormItem>
          <FormItem
            labelCol={{
              span: 5,
            }}
            wrapperCol={{
              span: 15,
            }}
            label="新密码"
          >
            {getFieldDecorator('password', {
              rules: [
                {
                  required: true,
                  message: '必须输入新密码!',
                },
                {
                  validator: this.checkConfirm,
                },
              ],
            })(<Input type="password" placeholder="请输入新密码" />)}
          </FormItem>
          <FormItem
            labelCol={{
              span: 5,
            }}
            wrapperCol={{
              span: 15,
            }}
            label="确认密码"
          >
            {getFieldDecorator('repwd', {
              rules: [
                {
                  required: true,
                  message: '必须输入确认密码!',
                },
                {
                  validator: this.checkPassword,
                },
              ],
            })(
              <Input
                type="password"
                placeholder="请输入再次输入新密码"
                onBlur={this.handleConfirmBlur}
              />
            )}
          </FormItem>
        </Form>
      </Modal>
    );
  }
}
