// ========================================
// 项目名称：节拍工厂·Takt Plat
// 命名空间：@/utils/crypto
// 文件名称：crypto.ts
// 创建时间：2026-05-26
// 创建人：Takt365(Cursor AI)
// 功能描述：登录密码 RSA 传输加密（crypto-js 编码 + JSEncrypt PKCS#1）
//
// 版权信息：Copyright (c) 2025 Takt  All rights reserved.
// 免责声明：此软件使用 MIT License，作者不承担任何使用风险。
// ========================================

import CryptoJS from 'crypto-js';
import { JSEncrypt } from 'jsencrypt';

/**
 * 使用 RSA 公钥加密登录密码（PKCS#1 v1.5，输出 Base64 密文）
 * @param {string} plainPassword 表单明文密码
 * @param {string} publicKeyPem 服务端下发的 RSA 公钥 PEM
 * @returns {string} Base64 密文；加密失败时返回空字符串
 */
export function encryptLoginPassword(plainPassword: string, publicKeyPem: string): string {
  if (!plainPassword || !publicKeyPem) {
    return '';
  }

  const encryptor = new JSEncrypt();
  encryptor.setPublicKey(publicKeyPem);

  const cipher = encryptor.encrypt(
    CryptoJS.enc.Utf8.stringify(CryptoJS.enc.Utf8.parse(plainPassword)),
  );

  return typeof cipher === 'string' ? cipher : '';
}
