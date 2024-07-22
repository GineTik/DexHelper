"use client"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar/Avatar"
import { Button } from "@/components/ui/button/Button"
import Link from "next/link"
import { BiSolidCopy } from "react-icons/bi"
import { IoIosTrendingUp } from "react-icons/io"
import styles from "./TokenHeader.module.scss"

type TokenHeaderProps = any

const TokenHeader = (data: TokenHeaderProps) => {
    return (
        <div className={styles.header}>
            <div className={styles.header__information}>
                <Avatar className={styles.header__image}>
                    <AvatarImage src={data.image} alt={data.symbol} />
                    <AvatarFallback>{data.symbol}</AvatarFallback>
                </Avatar>
                <div>
                    <div className={styles["header__name-row"]}>
                        <div className={styles.header__name}>{data.symbol}</div>
                        <div className={styles.header__symbol}> / {data.name}</div>
                    </div>
                    <div className={styles.header__address}>
                        <BiSolidCopy className={styles.address__icon} />
                        <span className={styles.address__text}>{data.tokenAddress}</span>
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
                {data.links?.map((o: any) => <Link href={o.url} className={styles.header__link}>
                    {o.name}
                </Link>)}
            </div>
        </div>
    )
}

export default TokenHeader
