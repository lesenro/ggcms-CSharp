import React, { PureComponent } from 'react';
import { Icon, Tag, Divider, Modal } from 'antd';
import moment from 'moment';
import groupBy from 'lodash/groupBy';
import Debounce from 'lodash-decorators/debounce';
import { Link } from 'dva/router';
import styles from './index.less';

const { confirm } = Modal;

export default class GlobalHeader extends PureComponent {
  state={currentUser:{}};
  componentWillUnmount() {
    this.triggerResizeEvent.cancel();
  }
  componentWillMount(){
    setTimeout(() => {
      this.props.dispatch({
        type: 'login/getLoginUser',
        callback: result => {
          if (result.Code === 0) {
            this.setState(
              {
                currentUser: result.Data
              }
            );
          }
        },
      });
    }, 100);
    

  }
  getNoticeData() {
    const { notices = [] } = this.props;
    if (notices.length === 0) {
      return {};
    }
    const newNotices = notices.map(notice => {
      const newNotice = { ...notice };
      if (newNotice.datetime) {
        newNotice.datetime = moment(notice.datetime).fromNow();
      }
      // transform id to item key
      if (newNotice.id) {
        newNotice.key = newNotice.id;
      }
      if (newNotice.extra && newNotice.status) {
        const color = {
          todo: '',
          processing: 'blue',
          urgent: 'red',
          doing: 'gold',
        }[newNotice.status];
        newNotice.extra = (
          <Tag color={color} style={{ marginRight: 0 }}>
            {newNotice.extra}
          </Tag>
        );
      }
      return newNotice;
    });
    return groupBy(newNotices, 'type');
  }
  toggle = () => {
    const { collapsed, onCollapse } = this.props;
    onCollapse(!collapsed);
    this.triggerResizeEvent();
  };
  /* eslint-disable*/
  @Debounce(600)
  triggerResizeEvent() {
    const event = document.createEvent('HTMLEvents');
    event.initEvent('resize', true, false);
    window.dispatchEvent(event);
  }
  render() {
    const {
      collapsed,
      isMobile,
      logo,
    } = this.props;

    const noticeData = this.getNoticeData();
    return (
      <div className={styles.header}>
        {isMobile && [
          <Link to="/" className={styles.logo} key="logo">
            <img src={logo} alt="logo" width="32" />
          </Link>,
          <Divider type="vertical" key="line" />,
        ]}
        <Icon
          className={styles.trigger}
          type={collapsed ? 'menu-unfold' : 'menu-fold'}
          onClick={this.toggle}/>
        <div className={styles.right}>
          <span className={styles.action}>
            {this.state.currentUser.UserName}
          </span>
          <a
            className={styles.action}
            onClick={ev => {
              ev.preventDefault();
              confirm({
                title: '退出确认',
                content: '确认要退出登录吗？',
                okText: '确认',
                cancelText: '取消',
                onOk: () => {
                  this.props.dispatch({
                    type: 'login/logout',
                  });
                },
                onCancel() {
                  //console.log('Cancel');
                },
              });
            }}
          >
            <Icon type="logout" /> <span> 退出</span>
          </a>
        </div>
      </div>
    );
  }
}
