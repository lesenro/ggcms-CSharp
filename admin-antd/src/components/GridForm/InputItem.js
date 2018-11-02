import React, { Component } from 'react';
import {
  Form,
  Input,
  Button,
  TreeSelect,
  Icon,
  Upload,
  Checkbox,
  Radio,
  Rate,
  Slider,
  Select,
  DatePicker,
  Calendar,
  InputNumber,
  Switch,
} from 'antd';
import styles from './InputItem.less';
import moment from 'moment';

const TreeNode = TreeSelect.TreeNode;
const { TextArea } = Input;
const FormItem = Form.Item;
const CheckboxGroup = Checkbox.Group;
const RadioGroup = Radio.Group;
const Option = Select.Option;
const GetTreeNodes = function(list) {
  return list.map(x => (
    <TreeNode {...x.Props} value={x.value} title={x.label} key={x.value}>
      {x.Children && x.Children.length > 0 && GetTreeNodes(x.Children)}
    </TreeNode>
  ));
};
@Form.create()
export default class InputItem extends Component {
  constructor(props) {
    super(props);
    this.state = {
      value: null,
      options: null,
      loading: false,
    };
    this.getValue = this.getValue.bind(this);
    this.validate = this.validate.bind(this);
    this.setValue = this.setValue.bind(this);
    this.setOptions = this.setOptions.bind(this);
    this.getError = this.getError.bind(this);
  }
  componentDidMount() {}
  componentWillMount() {
    const { item } = this.props;
    if (['CheckboxGroup', 'RadioGroup', 'Select', 'TreeSelect'].indexOf(item.InputType) != -1) {
      this.setState({ options: item.Options || [] });
    }
  }
  validate() {
    const { form, item } = this.props;
    form.validateFields([item.Name]);
  }

  getError() {
    const { form, item } = this.props;
    let errs = form.getFieldError(item.Name);
    if (errs && errs.length > 0) {
      return {
        name: item.Name,
        error: form.getFieldError(item.Name),
      };
    } else {
      return null;
    }
  }
  getValue() {
    const { form, item } = this.props;
    return {
      name: item.Name,
      value: form.getFieldValue(item.Name),
    };
  }
  setValue(val) {
    const { form, item } = this.props;
    let fixVal = val;
    if (item.InputType === 'Switch') {
      this.setState({ value: val });
    } else if (item.InputType === 'DateTimePicker') {
      fixVal = moment(val);
    } else if (item.InputType === 'Calendar') {
      fixVal = moment(val);
    }
    form.setFieldsValue({
      [item.Name]: fixVal,
    });
  }
  setOptions(opts) {
    const { form, item } = this.props;
    this.setState({ options: opts });
    let isString = typeof opts[0] === 'string';
    let val = form.getFieldValue(item.Name);
    let newval = [];
    if (val) {
      val.forEach(item => {
        if (isString) {
          if (opts.indexOf(item) != -1) {
            newval.push(item);
          }
        } else {
          let obj = opts.find(x => x.value === item);
          if (obj) {
            newval.push(obj.value);
          }
        }
      });
      form.setFieldsValue({
        [item.Name]: newval,
      });
    }
  }
  render() {
    const { form, item, global } = this.props;
    const { getFieldDecorator, ValidateCallback } = form;
    let input;
    if (item.InputType == 'Text') {
      input = (
        <Input
          prefix={item.Icon && <Icon type={item.Icon} className={styles.prefixIcon} />}
          {...item.Props}
        />
      );
    } else if (item.InputType == 'Password') {
      input = (
        <Input
          {...item.Props}
          type="password"
          prefix={item.Icon && <Icon type={item.Icon} className={styles.prefixIcon} />}
        />
      );
    } else if (item.InputType == 'TextArea') {
      input = <TextArea {...item.Props} />;
    } else if (item.InputType == 'DateTimePicker') {
      input = <DatePicker {...item.Props} />;
    } else if (item.InputType == 'Calendar') {
      input = <Calendar {...item.Props} />;
    } else if (item.InputType == 'Number') {
      input = <InputNumber {...item.Props} />;
    } else if (item.InputType == 'Switch') {
      input = (
        <Switch
          {...item.Props}
          checked={this.state.value}
          ref={c => {
            this.Ctrl = c;
          }}
        />
      );
    } else if (item.InputType == 'CheckboxGroup') {
      input = <CheckboxGroup {...item.Props} options={this.state.options || []} />;
    } else if (item.InputType == 'RadioGroup') {
      input = <RadioGroup {...item.Props} options={this.state.options || []} />;
    } else if (item.InputType == 'Select') {
      let options = this.state.options.map(x => {
        let o = x;
        if (typeof x == 'string') {
          o = {
            label: x,
            value: x,
          };
        }
        return (
          <Option {...o.Props} key={o.value}>
            {o.label}
          </Option>
        );
      });
      input = <Select {...item.Props}>{options}</Select>;
    } else if (item.InputType == 'Slider') {
      input = <Slider {...item.Props} />;
    } else if (item.InputType == 'Rate') {
      input = <Rate {...item.Props} />;
    } else if (item.InputType == 'TreeSelect') {
      let opts = GetTreeNodes(this.state.options);
      input = <TreeSelect {...item.Props}>{opts}</TreeSelect>;
    } else if (item.InputType == 'Upload') {
      const cfg = item.Config || {};
      const imageUrl = this.state.value;
      const uploadButton = imageUrl ? (
        <img src={imageUrl} alt="" />
      ) : (
        <div>
          <Icon type={this.state.loading ? 'loading' : 'plus'} />
          <div className="ant-upload-text">{cfg.Text || '头像'}</div>
        </div>
      );
      input = (
        <Upload {...item.Props}>
          {cfg.Type == 'btn' && (
            <Button {...cfg.Props}>
              <Icon type="upload" /> {cfg.Text || '上传'}
            </Button>
          )}
          {cfg.Type == 'avatar' && uploadButton}
        </Upload>
      );
    }
    if (input) {
      let itemProps = Object.assign({}, global.ItemProps, item.ItemProps);
      return (
        <FormItem {...itemProps}>
          {item.BeforeChildren && item.BeforeChildren}
          {getFieldDecorator(item.Name, {
            rules: item.Rules || [],
            getValueFromEvent: ev => {
              let val, ret;
              if (item.InputType === 'DateTimePicker') {
                if (ev) {
                  val = ev.toDate();
                  ret = ev;
                }
              } else if (item.InputType === 'Calendar') {
                if (ev) {
                  val = ev.toDate();
                  ret = ev;
                }
              } else if (item.InputType === 'Number') {
                if (ev) {
                  val = ev;
                  ret = ev;
                }
              } else if (item.InputType === 'Switch') {
                if (ev) {
                  val = ev;
                  ret = ev;
                }
              } else if (item.InputType === 'CheckboxGroup') {
                if (ev) {
                  val = ev;
                  ret = ev;
                }
              } else if (item.InputType === 'Select') {
                if (ev) {
                  val = ev;
                  ret = ev;
                }
              } else if (item.InputType === 'Slider') {
                if (ev) {
                  val = ev;
                  ret = ev;
                }
              } else if (item.InputType === 'Rate') {
                if (ev) {
                  val = ev;
                  ret = ev;
                }
              } else if (item.InputType === 'TreeSelect') {
                if (ev) {
                  val = ev;
                  ret = ev;
                }
              } else if (item.InputType === 'Upload') {
                console.log(ev);
                // 具体要根据服务器返回设置
                if (ev) {
                  val = ev;
                  ret = ev;
                }
              } else {
                val = ev.target.value;
                ret = val;
              }
              if (this.props.onChange) {
                this.props.onChange({
                  name: item.Name,
                  value: val,
                  event: ev,
                });
              }
              this.setState({ value: val });
              return ret;
            },
          })(input)}
          {item.AfterChildren && item.AfterChildren}
        </FormItem>
      );
    } else {
      return <span className="input-type-error" name={item.Name} />;
    }
  }
}
