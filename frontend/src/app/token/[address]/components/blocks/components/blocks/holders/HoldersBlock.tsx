import BlockHeader from "../../header/BlockHeader"
import BlockTitle from "../../title/BlockTitle"
import BaseBlockWrapper from "../base/BaseBlockWrapper"

const HoldersBlock = () => {
  return (
    <BaseBlockWrapper>
      <BlockHeader>
        <BlockTitle>Holders</BlockTitle>
      </BlockHeader>
    </BaseBlockWrapper>
  )
}

export default HoldersBlock
