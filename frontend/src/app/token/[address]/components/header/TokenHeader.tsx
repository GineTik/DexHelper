"use client"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar/Avatar"
import { Button } from "@/components/ui/button/Button"
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuLabel, DropdownMenuSeparator, DropdownMenuTrigger } from "@/components/ui/dropdown-menu/DropdownMenu"
import Link from "next/link"
import { BiHeart, BiSolidCopy } from "react-icons/bi"
import { BsPerson, BsThreeDotsVertical } from "react-icons/bs"
import { CgEye } from "react-icons/cg"
import { IoIosTrendingUp } from "react-icons/io"
import { MdOutlineBugReport } from "react-icons/md"
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
            <div className={styles.header__acts}>
                <Button 
                    className={styles.header__advertising} 
                    tooltip="profile of creator"
                    tooltipSide="bottom"
                >
                    <BsPerson />
                </Button>
                <Button 
                    className={styles.header__advertising} 
                    tooltip="up the token to the first line"
                    tooltipSide="bottom"
                >
                    <IoIosTrendingUp />
                </Button>
                <DropdownMenu>
					<DropdownMenuTrigger asChild>
                        <Button
                            className={styles.header__advertising} 
                            tooltip="more"
                            tooltipSide="bottom"
                        >
                            <BsThreeDotsVertical />
                        </Button>
					</DropdownMenuTrigger>
					<DropdownMenuContent side="right" align="start" sticky="always" className={styles.dropdown}>
						<DropdownMenuLabel>More actions</DropdownMenuLabel>
						<DropdownMenuSeparator />
						<DropdownMenuItem>
                            <BiHeart />
                            Favorite
                        </DropdownMenuItem>
                        <DropdownMenuItem>
                            <CgEye />
                            Hide
                        </DropdownMenuItem>
						<DropdownMenuSeparator />
						<DropdownMenuItem className={styles.dropdown__report}>
							<MdOutlineBugReport />
							Report
						</DropdownMenuItem>
					</DropdownMenuContent>
				</DropdownMenu>
            </div>
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
