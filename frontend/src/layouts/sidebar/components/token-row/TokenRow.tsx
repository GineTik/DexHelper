import { Button } from "@/components/ui/button/Button"
import { FiExternalLink } from "react-icons/fi"
import styles from "./TokenRow.module.scss"

const TokenRow = () => {
  return (
    <tr className={styles.token}>
      <td>
        <div className={styles.token__image}></div>
      </td>
      <td>
        <div className={styles.token__name}>BUSD</div>
        <div className={styles.token__symbol}>Binance USD</div>
      </td>
      <td>
        <div className={styles.token__mcap}>$14.7k</div>
        <div className={styles['token__mcap-percent']}>+350%</div>
      </td>
      <td className={styles.token__created}>3s</td>
      <td className={styles.token__actions}>
        <Button variant="icon" tooltip="to PhotonSol website" tooltipSide="right">
            <FiExternalLink className="text-white/50" />
        </Button>
      </td>
    </tr>
  )
}

export default TokenRow
