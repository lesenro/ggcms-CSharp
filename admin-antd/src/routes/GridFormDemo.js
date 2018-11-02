import React, { Component } from 'react';
import {
  Form,
  Input,
  message,
  Tabs,
  Button,
  Icon,
  Checkbox,
  Row,
  Col,
  Alert,
  InputNumber,
} from 'antd';
import { AppTools } from '../utils/utils';
import GridForm from '../components/GridForm/GridForm';
const demo = {
  Groups: {
    GroupType: 'tabs',
    Name: 'demo',
    Caption: '',
    Children: [
      {
        GroupType: 'row',
        Name: 'demo_1',
        Caption: '书签一',
        Children: [
          {
            GroupType: 'col',
            Name: 'demo_1_1',
            Caption: '分组一',
          },
          {
            GroupType: 'col',
            Name: 'demo_1_2',
            Caption: '分组二',
          },
        ],
      },
      {
        GroupType: 'tab_pane',
        Name: 'demo_2',
        Caption: '书签二',
        Children: [
          {
            GroupType: 'card',
            Name: 'demo_2_1',
            Caption: '卡片一',
          },
          {
            GroupType: 'card',
            Name: 'demo_2_2',
            Caption: '卡片二',
          },
        ],
      },
      {
        GroupType: 'tab_pane',
        Name: 'demo_3',
        Caption: '书签三',
        Children: [
          {
            GroupType: 'group_box',
            Name: 'demo_3_1',
            Caption: '盒子一',
          },
          {
            GroupType: 'box',
            Name: 'demo_3_2',
            Caption: '卡片二',
          },
        ],
      },
    ],
  },
  Items: [
    {
      InputType: 'Text',
      Name: 'UserName',
      GroupName: 'demo_1_1',
      Icon: 'user',
      ItemProps: {
        label: '用户名:',
      },
      Props: {
        placeholder: '请输入账户名',
        size: 'large',
        disabled: true,
      },
      Rules: [
        {
          max: 10,
          message: '帐号名应在10个字符以内',
        },
        {
          required: true,
          message: '请输入帐号名',
        },
        {
          whitespace: true,
          message: '请输入帐号名',
        },
      ],
    },
    {
      InputType: 'Password',
      Name: 'Password',
      Caption: '密码:',
      Icon: 'lock',
      GroupName: 'demo_1_1',
      Props: {
        placeholder: '请输入密码',
        size: 'large',
      },
      Rules: [
        {
          max: 12,
          message: '密码应在12个字符以内',
        },
        {
          required: true,
          message: '请输入密码',
        },
      ],
    },
    {
      InputType: 'TextArea',
      Name: 'Text',
      Caption: '说明:',
      GroupName: 'demo_3_1',
      Props: {
        placeholder: '请输入说明',
        rows: 4,
      },
    },
    {
      InputType: 'DateTimePicker',
      Name: 'DateTimePicker',
      Caption: '日期:',
      GroupName: 'demo_3_2',
      Props: {
        showTime: false,
        disabled: false,
        format: 'YYYY-MM-DD',
      },
    },
    {
      InputType: 'Calendar',
      Name: 'Calendar',
      Caption: '月历:',
      GroupName: 'demo_3_2',
      Props: {
        mode: 'month',
        // 不可选择的日期 (currentDate: moment) => boolean
        // "disabledDate": ,
        fullscreen: false,
      },
    },
    {
      InputType: 'Number',
      Name: 'Number',
      Caption: '数字:',
      GroupName: 'demo_3_2',
      Icon: 'api',
      Props: {
        size: 'large',
        disabled: false,
        step: 1,
        min: null,
        max: null,
        placeholder: '请输入数字',
      },
    },
    {
      InputType: 'CheckboxGroup',
      Name: 'Cbxlist1',
      Caption: '多选(string):',
      GroupName: 'demo_3_2',
      Options: ['选项一', '选项二', '选项三', '选项四'],
    },
    {
      InputType: 'CheckboxGroup',
      Name: 'Cbxlist2',
      Caption: '多选(object):',
      GroupName: 'demo_3_2',
      Options: [
        { label: 'Apple', value: 'Apple' },
        { label: 'Pear', value: 'Pear' },
        { label: 'Orange', value: 'Orange' },
      ],
    },
    {
      InputType: 'RadioGroup',
      Name: 'RadioGroup1',
      Caption: '单选:',
      GroupName: 'demo_1_2',
      Options: [
        { label: 'Apple', value: 'Apple' },
        { label: 'Pear', value: 'Pear' },
        { label: 'Orange', value: 'Orange' },
      ],
    },
    {
      InputType: 'RadioGroup',
      Name: 'RadioGroup2',
      Caption: '单选(string):',
      GroupName: 'demo_1_2',
      Options: ['选项一', '选项二', '选项三', '选项四'],
    },
    {
      InputType: 'Select',
      Name: 'ComboBox1',
      Caption: '单选:',
      GroupName: 'demo_1_2',
      Props: {
        disabled: false,
      },
      Options: [
        {
          label: 'Apple',
          value: 'Apple',
          Props: {
            disabled: true,
          },
        },
        { label: 'Pear', value: 'Pear' },
        { label: 'Orange', value: 'Orange' },
      ],
    },
    {
      InputType: 'Switch',
      Name: 'Switch1',
      GroupName: 'demo_1_2',
      ItemProps: { label: '开关:' },
      Props: {
        size: 'large',
        disabled: false,
        checkedChildren: '开',
        unCheckedChildren: '关',
      },
    },
    {
      InputType: 'Slider',
      Name: 'Slider1',
      GroupName: 'demo_1_2',
      ItemProps: { label: '滑条:' },
      Props: {
        range: true,
      },
    },
    {
      InputType: 'Rate',
      Name: 'Rate1',
      GroupName: 'demo_1_2',
      ItemProps: { label: '星级:' },
      Props: {
        range: true,
      },
    },
    {
      InputType: 'Upload',
      Name: 'Upload1',
      Config: {
        Type: 'btn',
        Text: '上传',
        Props: {},
      },
      GroupName: 'demo_1_2',
      ItemProps: { label: '上传   :' },
      Props: {
        name: 'file',
        action: '//jsonplaceholder.typicode.com/posts/',
        headers: {
          authorization: 'authorization-text',
        },
        onChange(info) {
          if (info.file.status !== 'uploading') {
            console.log(info.file, info.fileList);
          }
          if (info.file.status === 'done') {
            message.success(`${info.file.name} file uploaded successfully`);
          } else if (info.file.status === 'error') {
            message.error(`${info.file.name} file upload failed.`);
          }
        },
      },
    },
    {
      InputType: 'Upload',
      Name: 'Upload2',
      Config: {
        Type: 'avatar',
        Text: '图片y',
        Props: {},
      },
      GroupName: 'demo_1_2',
      ItemProps: { label: '头像:' },
      Props: {
        className: 'avatar-uploader',
        listType: 'picture-card',
        name: 'file',
      },
    },
    {
      InputType: 'TreeSelect',
      Name: 'TreeSelect1',
      GroupName: 'demo_1_2',
      ItemProps: { label: '树选择器:' },
      Props: {
        size: 'large',
      },
      Options: [
        {
          label: '动物',
          value: 'r_1',
          Children: [
            { label: '狮子', value: 'r_1_1' },
            { label: '老虎', value: 'r_1_2' },
            { label: '大象', value: 'r_1_3' },
          ],
        },
        {
          label: '植物',
          value: 'r_2',
          Props: { disabled: true },
          Children: [
            { label: '水果', value: 'r_2_1' },
            { label: '蔬菜', value: 'r_2_2' },
            { label: '树木', value: 'r_2_3' },
          ],
        },
        {
          label: '其他',
          value: 'r_3',
          Children: [
            { label: '山', value: 'r_3_1' },
            { label: '水', value: 'r_3_2' },
            { label: '石头', value: 'r_3_3' },
          ],
        },
      ],
    },
  ],
  Global: {
    GroupsProps: {},
    ItemProps: {
      labelCol: { span: 6 },
      wrapperCol: { span: 14 },
    },
    ElementProps: {},
    SubmitItemProps: {},
    SubmitButtonProps: {
      size: 'large',
      type: 'primary',
    },
  },
};
export default class GridFormDemo extends Component {
  constructor(props) {
    super(props);
    this.state = {
      demo: demo,
    };
  }
  handleSubmit1 = ev => {
    let val = AppTools.arrayToObject(ev);
    console.log(val);
  };
  render() {
    const { title } = this.props;
    return (
      <div>
        <GridForm
          tmpl={this.state.demo}
          submit={this.handleSubmit1}
          onItemChange={ev => {
            console.log(ev);
          }}
          onError={ev => {
            console.log(ev);
          }}
          ref={frm => {
            this.editFrm = frm;
          }}
        />
        <Button
          size="large"
          type="primary"
          onClick={ev => {
            this.editFrm.SetValues({
              UserName: 'bbb',
              Password: 'ccc',
              Switch1: true,
              Text: 'TextTextText',
              DateTimePicker: new Date(),
              Calendar: '2017-08-05',
              Number: 23,
              // Cbxlist1:["选项一","选项三"]
              RadioGroup1: 'Orange',
              ComboBox1: 'Orange',
              TreeSelect1: 'r_2_2',
            });
            this.editFrm.SetOptions({
              Cbxlist1: ['一', '二', '三', '四', '选项三'],
              Cbxlist2: [
                { label: 'Apple1', value: 'Apple1' },
                { label: 'Pear', value: 'Pear' },
                { label: 'Orange2', value: 'Orange2' },
              ],
            });
          }}
        >
          {' '}
          更新1{' '}
        </Button>
        <Button
          size="large"
          type="primary"
          onClick={ev => {
            let d = this.state.demo;
            d.Items[0].ItemProps.label = '帐号:';
            d.Items[0].Props.disabled = false;
            this.setState({
              demo: d,
            });
          }}
        >
          {' '}
          更新2{' '}
        </Button>
      </div>
    );
  }
}
