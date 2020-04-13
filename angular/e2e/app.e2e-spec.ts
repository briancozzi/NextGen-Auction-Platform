import { AuctionTemplatePage } from './app.po';

describe('Auction App', function() {
  let page: AuctionTemplatePage;

  beforeEach(() => {
    page = new AuctionTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
