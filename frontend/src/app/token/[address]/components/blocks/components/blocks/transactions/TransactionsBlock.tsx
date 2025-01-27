import BlockHeader from "../../header/BlockHeader"
import BlockTitle from "../../title/BlockTitle"
import BaseBlockWrapper from "../base/BaseBlockWrapper"

type TransactionsBlockProps = {}

const TransactionsBlock = ({}: TransactionsBlockProps) => {
  return (
    <BaseBlockWrapper>
      <BlockHeader>
        <BlockTitle>Transactions</BlockTitle>
      </BlockHeader>
    </BaseBlockWrapper>
  )
}

export default TransactionsBlock
