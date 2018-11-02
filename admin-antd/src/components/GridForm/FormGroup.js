import React, { Component } from 'react';
import { format } from 'path';
import { Tabs, Collapse, Card, Row, Col } from 'antd';
import GroupBox from './GroupBox';
import InputItem from './InputItem';
const { TabPane } = Tabs;
const Panel = Collapse.Panel;

export default class FormGroup extends Component {
  Inputs = [];
  constructor(props) {
    super(props);
  }

  Validate() {
    const { group, items } = this.props;
    let values = [];
    if (group.Children) {
      group.Children.forEach(item => {
        if (item.ctrl) {
          values.push(item.ctrl.Validate());
        }
      });
    }
    let grp_items = (items || []).filter(x => x.GroupName === group.Name);
    if (grp_items.length > 0) {
      grp_items.forEach(item => {
        if (item.ctrl) {
          values.push(item.ctrl.validate());
        }
      });
    }
    return values;
  }
  GetValues() {
    const { group, items } = this.props;
    let values = [];
    if (group.Children) {
      group.Children.forEach(item => {
        if (item.ctrl) {
          values.push(item.ctrl.GetValues());
        }
      });
    }
    let grp_items = (items || []).filter(x => x.GroupName === group.Name);
    if (grp_items.length > 0) {
      grp_items.forEach(item => {
        if (item.ctrl) {
          values.push(item.ctrl.getValue());
        }
      });
    }
    return values;
  }
  GetErrors() {
    const { group, items } = this.props;
    let values = [];
    if (group.Children) {
      group.Children.forEach(item => {
        if (item.ctrl) {
          values.push(item.ctrl.GetErrors());
        }
      });
    }
    let grp_items = (items || []).filter(x => x.GroupName === group.Name);
    if (grp_items.length > 0) {
      grp_items.forEach(item => {
        if (item.ctrl) {
          values.push(item.ctrl.getError());
        }
      });
    }
    return values;
  }
  SetValue(name, val) {
    const { group, items } = this.props;
    let isOk = false;
    if (group.Children) {
      group.Children.forEach(item => {
        if (item.ctrl) {
          if (item.ctrl.SetValue(name, val)) {
            isOk = true;
            return;
          }
        }
      });
    }
    let grp_items = (items || []).filter(x => x.GroupName === group.Name);
    if (grp_items.length > 0) {
      grp_items.forEach(item => {
        if (item.Name === name && item.ctrl) {
          item.ctrl.setValue(val);
          isOk = true;
          return;
        }
      });
    }
    return isOk;
  }
  SetOptions(name, val) {
    const { group, items } = this.props;
    let isOk = false;
    if (group.Children) {
      group.Children.forEach(item => {
        if (item.ctrl) {
          if (item.ctrl.SetOptions(name, val)) {
            isOk = true;
            return;
          }
        }
      });
    }
    let grp_items = (items || []).filter(x => x.GroupName === group.Name);
    if (grp_items.length > 0) {
      grp_items.forEach(item => {
        if (item.Name === name && item.ctrl) {
          item.ctrl.setOptions(val);
          isOk = true;
          return;
        }
      });
    }
    return isOk;
  }

  render() {
    const { group, items, onChange, global } = this.props;
    let Children, Inputs;
    let grp_items = (items || []).filter(x => x.GroupName === group.Name);
    if (grp_items.length > 0) {
      Inputs = grp_items.map((item, key) => (
        <InputItem
          onChange={ev => {
            if (onChange) {
              onChange(ev);
            }
          }}
          global={global}
          item={item}
          key={key}
          wrappedComponentRef={x => (item.ctrl = x)}
        />
      ));
    }
    if (group.GroupType === 'tabs') {
      if (group.Children) {
        Children = group.Children.map((item, key) => (
          <TabPane forceRender={true} tab={item.Caption} key={key}>
            <FormGroup
              onChange={onChange}
              global={global}
              items={items}
              group={item}
              ref={x => (item.ctrl = x)}
            />
          </TabPane>
        ));
      }
      return <Tabs>{Children && Children}</Tabs>;
    } else if (group.GroupType === 'collapse') {
      if (group.Children) {
        Children = group.Children.map((item, key) => (
          <Panel header={item.Caption} key={key}>
            <FormGroup
              onChange={onChange}
              global={global}
              items={items}
              group={item}
              ref={x => (item.ctrl = x)}
            />
          </Panel>
        ));
      }
      return <Collapse>{Children && Children}</Collapse>;
    } else if (group.GroupType === 'row') {
      if (group.Children) {
        Children = group.Children.map((item, key) => (
          <Col {...item.Props} key={key}>
            <FormGroup
              onChange={onChange}
              global={global}
              items={items}
              group={item}
              ref={x => (item.ctrl = x)}
            />
          </Col>
        ));
      }
      return <Row>{Children && Children}</Row>;
    } else if (group.GroupType === 'card') {
      if (group.Children) {
        Children = group.Children.map((item, key) => (
          <FormGroup
            global={global}
            onChange={onChange}
            items={items}
            group={item}
            key={key}
            ref={x => (item.ctrl = x)}
          />
        ));
      }
      return (
        <Card title={group.Caption}>
          {Children && Children}
          {Inputs && Inputs}
        </Card>
      );
    } else if (group.GroupType === 'group_box') {
      if (group.Children) {
        Children = group.Children.map((item, key) => (
          <FormGroup
            global={global}
            onChange={onChange}
            items={items}
            group={item}
            key={key}
            ref={x => (item.ctrl = x)}
          />
        ));
      }
      return (
        <GroupBox title={group.Caption}>
          {Children && Children}
          {Inputs && Inputs}
        </GroupBox>
      );
    } else {
      if (group.Children) {
        Children = group.Children.map((item, key) => (
          <FormGroup
            global={global}
            onChange={onChange}
            items={items}
            group={item}
            key={key}
            ref={x => (item.ctrl = x)}
          />
        ));
      }
      return (
        <div className="form-group">
          {Children && Children}
          {Inputs && Inputs}
        </div>
      );
    }
  }
}
