import BlockHeader from "../../header/BlockHeader"
import BlockTitle from "../../title/BlockTitle"
import BaseBlockWrapper from "../base/BaseBlockWrapper"
import styles from "./InformationBlock.module.scss"

type InformationBlockProps = {}

const InformationBlock = ({}: InformationBlockProps) => {
  return (
    <BaseBlockWrapper className={styles.information}>
		<BlockHeader>
			<BlockTitle>Information</BlockTitle>
		</BlockHeader>
		<div className={styles.information__row}>
			<div className={styles.information__block}>
				<div className={styles.information__label}>Description</div>
				<div className={styles.information__value}>Master of School of Memes</div>
			</div>
		</div>
		<div className={styles.information__row}>
			<div className={styles.information__block}>
				<div className={styles.information__label}>Price USD</div>
				<div className={styles.information__value}>$0.0₃1470</div>
			</div>
			<div className={styles.information__block}>
				<div className={styles.information__label}>PRICE SOL</div>
				<div className={styles.information__value}>0.0₅1144</div>
			</div>
		</div>
		<div className={styles.information__row}>
			<div className={styles.information__block}>
				<div className={styles.information__label}>MKT CAP</div>
				<div className={styles.information__value}>$183.64K</div>
			</div>
			<div className={styles.information__block}>
				<div className={styles.information__label}>LIQUIDITY</div>
				<div className={styles.information__value}>$39K</div>
			</div>
		</div>
		<div className={styles.information__row}>
			<div className={styles.information__block}>
				<div className={styles.information__label}>HOLDERS</div>
				<div className={styles.information__value}>532</div>
			</div>
		</div>
		<div className={styles.information__row}>
			<div className={styles.information__block}>
				<div className={styles.information__label}>Tracked wallets</div>
				<div className={styles.information__value}>
					<div className={styles.wallets}>
						<div className={styles.wallets__avatar}></div>
						<div className={styles.wallets__avatar}></div>
						<div className={styles.wallets__avatar}>+3</div>
					</div>
					<div>bought the token</div>
				</div>
			</div>
		</div>
    </BaseBlockWrapper>
  )
}

export default InformationBlock
