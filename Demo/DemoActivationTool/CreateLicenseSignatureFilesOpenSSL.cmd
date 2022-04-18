openssl.exe req -x509 -nodes -sha256 -days 3650 -subj "/CN=LicenseSign" -newkey rsa:2048 -keyout LicenseSign.key -out LicenseSign.crt
openssl.exe pkcs12 -export -in LicenseSign.crt -inkey LicenseSign.key -CSP "Microsoft Enhanced RSA and AES Cryptographic Provider" -out LicenseSign.pfx
openssl.exe x509 -in LicenseSign.crt -outform DER -out  LicenseVerify.cer