import { NextGen-Auction-PlatformTemplatePage } from './app.po';

describe('NextGen-Auction-Platform App', function() {
  let page: NextGen-Auction-PlatformTemplatePage;

  beforeEach(() => {
    page = new NextGen-Auction-PlatformTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
