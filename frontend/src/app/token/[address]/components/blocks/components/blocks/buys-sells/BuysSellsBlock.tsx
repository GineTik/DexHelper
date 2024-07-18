import BlockHeader from '../../header/BlockHeader'
import BlockTitle from '../../title/BlockTitle'
import BaseBlockWrapper from '../base/BaseBlockWrapper'

const BuysSellsBlock = () => {
  return (
    <BaseBlockWrapper>
      <BlockHeader>
        <BlockTitle>Buys/Sells</BlockTitle>
      </BlockHeader>
    </BaseBlockWrapper>
  )
}

export default BuysSellsBlock
