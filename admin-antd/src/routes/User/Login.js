import React, { Component } from 'react';
import { connect } from 'dva';
import { notification, Alert, Tooltip } from 'antd';
import GridForm from '../../components/GridForm/GridForm';
import styles from './Login.less';
import { AppTools } from '../../utils/utils';
import { CaptchaUrl } from '../../services/platformApi';


const loginForm = {
  Groups: {
    GroupType: 'box',
    Name: 'form1',
    Caption: '登录框',
  },
  Items: [
    {
      InputType: 'Text',
      Name: 'username',
      GroupName: 'form1',
      Icon: 'user',
      ItemProps: {
        // "label": "用户名:"
      },
      Props: {
        placeholder: '请输入账户名',
        size: 'large',
        // "disabled": true,
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
      Name: 'password',
      Caption: '密码:',
      Icon: 'lock',
      GroupName: 'form1',
      ItemProps: {
        // "label": "密码:"
      },
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
      InputType: 'Text',
      Name: 'captcha',
      Caption: '验证码:',
      Icon: 'safety',
      GroupName: 'form1',
      ItemProps: {
        // "label": "密码:"
      },
      Props: {
        placeholder: '请输入验证码',
        size: 'large',
        style: {
          width: '250px',
        },
      },
      Rules: [
        {
          len: 4,
          message: '验证码应为4个字符',
        },
        {
          required: true,
          message: '请输入验证码',
        },
      ],
    },
  ],
  Global: {
    GroupsProps: {},
    ItemProps: {
      // labelCol: { span: 6 },
      // wrapperCol: { span: 14 },
    },
    ElementProps: {},
    SubmitItemProps: {
      className: styles.additional,
    },
    SubmitButtonProps: {
      size: 'large',
      className: styles.submit,
      type: 'primary',
      text: '登录',
    },
  },
};

@connect(state => ({
  login: state.login,
}))
export default class Login extends Component {
  constructor(props) {
    super(props);

    this.state = {
      count: 0,
      loginForm,
    };
    this.getCaptcha = this.getCaptcha.bind(this);
  }
  componentWillReceiveProps() {
    // this.getCaptcha();
  }
  componentWillMount() {
    const lfrm = loginForm;
    const imgItem = lfrm.Items.find(x => x.Name === 'captcha');
    if (imgItem) {
      imgItem.BeforeChildren = (
        <Tooltip placement="top" title="看不清,换一换">
          <img
            ref={
              img=>this.refCodeImg=img
            }
            className={styles.codeImg}
            onClick={() => {
              this.getCaptcha();
            }}
          />
        </Tooltip>
      );
    }
  }
  componentDidMount() {
    this.getCaptcha();
  }


  handleSubmit = ev => {
    const vals = AppTools.arrayToObject(ev);
    this.props.dispatch({
      type: `login/adminLogin`,
      payload: vals,
      callback: result => {
        if (result.Code !== 0) {
          notification.error({
            message: "登录失败",
            description: result.Msg,
          });
          this.getCaptcha();
        }
      },
    });
  };

  renderMessage = message => {
    return <Alert style={{ marginBottom: 24 }} message={message} type="error" showIcon />;
  };

  getCaptcha() {
    const imgurl=CaptchaUrl();
    this.refCodeImg.src=imgurl;
    this.editFrm.SetValues({captcha:''})

    this.setState(
      {
        count: this.state.count + 1,
      }
    );
  }
  render() {
    const { login } = this.props;
    return (
      <div className={styles.main}>
        {login.status === 'error' &&
          login.type === 'account' &&
          login.submitting === false &&
          this.renderMessage('账户或密码错误')}
        <GridForm
          onError={ev => {
            console.log(ev);
          }}
          loading={login.submitting}
          tmpl={this.state.loginForm}
          submit={this.handleSubmit}
          ref={frm => {
            this.editFrm = frm;
          }}
        />
      </div>
    );
  }
}
