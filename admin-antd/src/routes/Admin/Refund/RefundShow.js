import React, { PureComponent } from 'react';
import { Form, Icon, Input, Button, Checkbox, Modal, Select, Tree, Switch } from 'antd';

const Option = Select.Option;
const TreeNode = Tree.TreeNode;
const CheckboxGroup = Checkbox.Group;
const FormItem = Form.Item;
const { TextArea } = Input;

@Form.create()
export default class RefundShow extends PureComponent {
  state = {
    checkedMenus: [],
    menu: [],
  };

  handleSubmit = e => {
    e.preventDefault();
    this.props.form.validateFields((err, values) => {
      if (!err) {
        var senddata = {
          ...values,
          id: this.state.id,
          state: values.state ? 0 : 1,
        };
        this.props.onSubmit({ type: 'success', data: senddata });
      }
    });
  };
  //   componentWillReceiveProps(nextProps) {     //this.props.form.resetFields();
  //     this.setState({...nextProps.data},()=>{     });   }
  componentDidMount() {
    this.setState({
      ...this.props.data,
    });
  }
  render() {
    const { getFieldDecorator } = this.props.form;
    const { roles } = this.props;
    var datainfo = this.state;
    const rolesOption = roles.map(x => {
      return (
        <Option key={x.id} value={x.id}>
          {' '}
          {x.name}{' '}
        </Option>
      );
    });
    //resetFields
    return (
      <Modal
        title={datainfo.id ? '编辑用户' : '新建用户'}
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
            label="名称"
          >
            {!datainfo.id &&
              getFieldDecorator('loginName', {
                initialValue: datainfo.loginName,
                rules: [
                  {
                    required: true,
                    message: '必须输入用户名称!',
                  },
                ],
              })(<Input placeholder="请输入用户名称" />)}
            {datainfo.id &&
              getFieldDecorator('loginName', {
                initialValue: datainfo.loginName,
              })(<Input disabled />)}
          </FormItem>
          <FormItem
            labelCol={{
              span: 5,
            }}
            wrapperCol={{
              span: 15,
            }}
            label="密码"
          >
            {!datainfo.id &&
              getFieldDecorator('password', {
                initialValue: datainfo.password,
                rules: [
                  {
                    required: true,
                    message: '必须输入密码!',
                  },
                ],
              })(<Input type="password" placeholder="请输入密码" />)}
            {datainfo.id &&
              getFieldDecorator('password')(<Input type="password" placeholder="请输入密码" />)}
          </FormItem>

          <FormItem
            labelCol={{
              span: 5,
            }}
            wrapperCol={{
              span: 15,
            }}
            label="邮箱"
          >
            {getFieldDecorator('email', {
              initialValue: datainfo.email,
            })(<Input type="email" placeholder="请输入邮箱" />)}
          </FormItem>
          <FormItem
            labelCol={{
              span: 5,
            }}
            wrapperCol={{
              span: 15,
            }}
            label="手机"
          >
            {getFieldDecorator('mobile', {
              initialValue: datainfo.mobile,
              rules: [
                {
                  pattern: /^1[3|5|7|8|][0-9]{9}$/,
                  message: '请输入正确的手机号码!',
                },
              ],
            })(<Input placeholder="请输入手机" />)}
          </FormItem>
          <FormItem
            labelCol={{
              span: 5,
            }}
            wrapperCol={{
              span: 15,
            }}
            label="用户角色"
          >
            {getFieldDecorator('role', {
              initialValue: datainfo.role,
              rules: [
                {
                  required: true,
                  message: '必须选择用户角色!',
                },
              ],
            })(
              <Select
                style={{
                  width: 200,
                }}
                onChange={ev => {
                  this.setState({ icon: ev });
                }}
              >
                <Option value="">--请选择--</Option>
                {rolesOption}
              </Select>
            )}
          </FormItem>
          <FormItem
            labelCol={{
              span: 5,
            }}
            wrapperCol={{
              span: 15,
            }}
            label="是否禁用"
          >
            {getFieldDecorator('state', {
              initialValue: datainfo.state == 0,
              valuePropName: 'checked',
            })(<Switch checkedChildren="正常" unCheckedChildren="禁用" />)}
          </FormItem>
        </Form>
      </Modal>
    );
  }
}
