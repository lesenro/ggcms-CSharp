import { GgcmsAppPage } from './app.po';

describe('ggcms-app App', () => {
  let page: GgcmsAppPage;

  beforeEach(() => {
    page = new GgcmsAppPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
