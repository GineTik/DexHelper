import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar/Avatar"
import { Button } from "@/components/ui/button/Button"
import Link from "next/link"
import { BiSolidCopy } from "react-icons/bi"
import { IoIosTrendingUp } from "react-icons/io"
import styles from "./TokenHeader.module.scss"

const TokenHeader = () => {
  return (
    <div className={styles.header}>
        <div className={styles.header__information}>
            <Avatar className={styles.header__image}>
                <AvatarImage src={"https://gateway.irys.xyz/MRGsNYI6211hAzYD_S8WcfMn48fiOOzfEHj5OVVeL7c"} alt={"BUSD"} />
                <AvatarFallback>BUSD</AvatarFallback>
            </Avatar>
            <div>
                <div className={styles["header__name-row"]}>
                    <div className={styles.header__name}>BUSD</div>
                    <div className={styles.header__symbol}> / Binance USD</div>
                </div>
                <div className={styles.header__address}>
                    <BiSolidCopy />
                    <span>BF4jcAa8mbH4Bi...</span>
                </div>
            </div>
        </div>
        <Button 
            className={styles.header__advertising} 
            tooltip="up the token to the first line"
            tooltipSide="bottom"
        >
            <IoIosTrendingUp className="text-white/50" />
        </Button>
        <div className={styles.header__links}>
            <Link href={""} className={styles.header__link}>
                PhotonSol
            </Link>
            <Link href={""} className={styles.header__link}>
                PumpFun
            </Link>
            <Link href={""} className={styles.header__link}>
                Telegram
            </Link>
            <Link href={""} className={styles.header__link}>
                Twitter
            </Link>
            <Link href={""} className={styles.header__link}>
                Website
            </Link>
        </div>
    </div>
  )
}

export default TokenHeader
