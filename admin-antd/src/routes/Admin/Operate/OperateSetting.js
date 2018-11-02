import { Form, Icon, Input, Button, Checkbox, Divider, Select, Radio } from 'antd';
const FormItem = Form.Item;
const Option = Select.Option;
const RadioGroup = Radio.Group;
export const iconArray = [
  'step-backward',
  'step-forward',
  'fast-backward',
  'fast-forward',
  'shrink',
  'arrows-alt',
  'down',
  'up',
  'left',
  'right',
  'caret-up',
  'caret-down',
  'caret-left',
  'caret-right',
  'up-circle',
  'down-circle',
  'left-circle',
  'right-circle',
  'up-circle-o',
  'down-circle-o',
  'right-circle-o',
  'left-circle-o',
  'double-right',
  'double-left',
  'verticle-left',
  'verticle-right',
  'forward',
  'backward',
  'rollback',
  'enter',
  'retweet',
  'swap',
  'swap-left',
  'swap-right',
  'arrow-up',
  'arrow-down',
  'arrow-left',
  'arrow-right',
  'play-circle',
  'play-circle-o',
  'up-square',
  'down-square',
  'left-square',
  'right-square',
  'up-square-o',
  'down-square-o',
  'left-square-o',
  'right-square-o',
  'login',
  'logout',
  'menu-fold',
  'menu-unfold',
  'question',
  'question-circle-o',
  'question-circle',
  'plus',
  'plus-circle-o',
  'plus-circle',
  'pause',
  'pause-circle-o',
  'pause-circle',
  'minus',
  'minus-circle-o',
  'minus-circle',
  'plus-square',
  'plus-square-o',
  'minus-square',
  'minus-square-o',
  'info',
  'info-circle-o',
  'info-circle',
  'exclamation',
  'exclamation-circle-o',
  'exclamation-circle',
  'close',
  'close-circle',
  'close-circle-o',
  'close-square',
  'close-square-o',
  'check',
  'check-circle',
  'check-circle-o',
  'check-square',
  'check-square-o',
  'clock-circle-o',
  'clock-circle',
  'warning',
  'lock',
  'unlock',
  'area-chart',
  'pie-chart',
  'bar-chart',
  'dot-chart',
  'bars',
  'book',
  'calendar',
  'cloud',
  'cloud-download',
  'code',
  'code-o',
  'copy',
  'credit-card',
  'delete',
  'desktop',
  'download',
  'edit',
  'ellipsis',
  'file',
  'file-text',
  'file-unknown',
  'file-pdf',
  'file-excel',
  'file-jpg',
  'file-ppt',
  'file-add',
  'folder',
  'folder-open',
  'folder-add',
  'hdd',
  'frown',
  'frown-o',
  'meh',
  'meh-o',
  'smile',
  'smile-o',
  'inbox',
  'laptop',
  'appstore-o',
  'appstore',
  'line-chart',
  'link',
  'mail',
  'mobile',
  'notification',
  'paper-clip',
  'picture',
  'poweroff',
  'reload',
  'search',
  'setting',
  'share-alt',
  'shopping-cart',
  'tablet',
  'tag',
  'tag-o',
  'tags',
  'tags-o',
  'to-top',
  'upload',
  'user',
  'video-camera',
  'home',
  'loading',
  'loading-3-quarters',
  'cloud-upload-o',
  'cloud-download-o',
  'cloud-upload',
  'cloud-o',
  'star-o',
  'star',
  'heart-o',
  'heart',
  'environment',
  'environment-o',
  'eye',
  'eye-o',
  'camera',
  'camera-o',
  'save',
  'team',
  'solution',
  'phone',
  'filter',
  'exception',
  'export',
  'customer-service',
  'qrcode',
  'scan',
  'like',
  'like-o',
  'dislike',
  'dislike-o',
  'message',
  'pay-circle',
  'pay-circle-o',
  'calculator',
  'pushpin',
  'pushpin-o',
  'bulb',
  'select',
  'switcher',
  'rocket',
  'bell',
  'disconnect',
  'database',
  'compass',
  'barcode',
  'hourglass',
  'key',
  'flag',
  'layout',
  'printer',
  'sound',
  'usb',
  'skin',
  'tool',
  'sync',
  'wifi',
  'car',
  'schedule',
  'user-add',
  'user-delete',
  'usergroup-add',
  'usergroup-delete',
  'man',
  'woman',
  'shop',
  'gift',
  'idcard',
  'medicine-box',
  'red-envelope',
  'coffee',
  'copyright',
  'trademark',
  'safety',
  'wallet',
  'bank',
  'trophy',
  'contacts',
  'global',
  'shake',
  'api',
  'fork',
  'dashboard',
  'form',
  'table',
  'profile',
  'android',
  'android-o',
  'apple',
  'apple-o',
  'windows',
  'windows-o',
  'ie',
  'chrome',
  'github',
  'aliwangwang',
  'aliwangwang-o',
  'dingding',
  'dingding-o',
  'weibo-square',
  'weibo-circle',
  'taobao-circle',
  'html5',
  'weibo',
  'twitter',
  'wechat',
  'youtube',
  'alipay-circle',
  'taobao',
  'skype',
  'qq',
  'medium-workmark',
  'gitlab',
  'medium',
  'linkedin',
  'google-plus',
  'dropbox',
  'facebook',
  'codepen',
  'amazon',
  'google',
  'codepen-circle',
  'alipay',
  'ant-design',
];
export class OperateHelper {
  static defaultOption = {
    position: 'top',
    icon: '',
    btn_type: 'primary',
    size: 'default',
    className: '',
  };
  static getOption(tmpl) {
    try {
      var opt = JSON.parse(tmpl || '{}');
      return Object.assign({}, this.defaultOption, opt);
    } catch (ex) {
      return this.defaultOption;
    }
  }
  static operateButtonShow(btns, buttonClick, record) {
    var pos = record ? 'record' : 'top';
    return (
      <div>
        {btns.map(item => {
          var opt = this.getOption(item.templateContent);
          if (opt.position === pos) {
            return (
              <Button
                onClick={ev => {
                  ev.preventDefault();
                  ev.stopPropagation();
                  buttonClick({
                    ...item,
                    record: record,
                  });
                }}
                key={item.id}
                size={opt.size}
                type={opt.btn_type}
                className={opt.className}
                icon={opt.icon}
              >
                {item.name}
              </Button>
            );
          } else {
            return [<span key={item.id} />];
          }
        })}
      </div>
    );
  }
}
var iconsOption = iconArray.map(x => {
  return (
    <Option key={x} value={x}>
      <Icon type={x} /> {x}{' '}
    </Option>
  );
});
export class OperateSetting extends React.Component {
  state = OperateHelper.defaultOption;
  handleChange() {
    //this.props.onChange(JSON.stringify(this.state));
  }
  getSettingString() {
    return JSON.stringify(this.state);
  }
  componentWillReceiveProps(nextProps) {
    var opt = OperateHelper.getOption(nextProps.value);
    this.setState({ ...opt });
  }
  componentDidMount() {
    var opt = OperateHelper.getOption(this.props.value);
    this.setState({ ...opt });
  }
  render() {
    return (
      <div>
        <Divider>操作参数</Divider>
        <FormItem labelCol={{ span: 5 }} wrapperCol={{ span: 15 }} label="位置">
          <RadioGroup
            value={this.state.position}
            defaultValue={this.state.position}
            onChange={ev => {
              this.setState({ position: ev.target.value }, () => {
                this.handleChange();
              });
            }}
          >
            <Radio value="top">顶部</Radio>
            <Radio value="record">记录</Radio>
            <Radio value="hidden">不显示</Radio>
          </RadioGroup>
        </FormItem>
        <FormItem labelCol={{ span: 5 }} wrapperCol={{ span: 15 }} label="图标">
          <Select
            value={this.state.icon}
            defaultValue={this.state.icon}
            style={{ width: 200 }}
            onChange={ev => {
              this.setState({ icon: ev }, () => {
                this.handleChange();
              });
            }}
          >
            <Option value=""> 无图标 </Option>
            {iconsOption}
          </Select>
        </FormItem>
        <FormItem labelCol={{ span: 5 }} wrapperCol={{ span: 15 }} label="按钮尺寸">
          <Select
            value={this.state.size}
            defaultValue={this.state.size}
            style={{ width: 200 }}
            onChange={ev => {
              this.setState({ size: ev }, () => {
                this.handleChange();
              });
            }}
          >
            <Option value="large">大</Option>
            <Option value="default">中</Option>
            <Option value="small">小</Option>
          </Select>
        </FormItem>
        <FormItem labelCol={{ span: 5 }} wrapperCol={{ span: 15 }} label="类型样式">
          <Select
            value={this.state.btn_type}
            defaultValue={this.state.btn_type}
            style={{ width: 200 }}
            onChange={ev => {
              this.setState({ btn_type: ev }, () => {
                this.handleChange();
              });
            }}
          >
            <Option value="primary">主按钮</Option>
            <Option value="">次按钮</Option>
            <Option value="dashed">虚线按钮</Option>
            <Option value="danger">危险按钮</Option>
          </Select>
        </FormItem>
        <FormItem labelCol={{ span: 5 }} wrapperCol={{ span: 15 }} label="自定义样式">
          <Input
            value={this.state.className}
            defaultValue={this.state.className}
            placeholder="自定义样式名称"
            onChange={ev => {
              this.setState({ className: ev.target.value }, () => {
                this.handleChange();
              });
            }}
          />
        </FormItem>
      </div>
    );
  }
}
